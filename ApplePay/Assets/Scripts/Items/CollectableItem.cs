using UnityEngine;

public abstract class CollectableItem<Item> : CollectableObject
{
    [UnityEngine.SerializeField] protected ItemHoverableObject hoverableObject;
    protected virtual string hoverableObjectHeader {get => null;}
    protected virtual string hoverableObjectDescription {get => null;}
    protected abstract Item CollectableObject { get; set; }
    protected abstract string TargetRepository { get; }
    protected override void Start()
    {
        base.Start();
        if(hoverableObjectHeader != null && hoverableObjectDescription != null)
            hoverableObject.Init(transform);
    }
    protected override void Update()
    {
        base.Update();
        if(hoverableObjectHeader != null && hoverableObjectDescription != null)
            hoverableObject.Update(hoverableObjectHeader, hoverableObjectDescription);
    }
    private void OnMouseEnter()
    {
        if(hoverableObject.Initiated)
            hoverableObject.OnMouseEnter(hoverableObjectHeader, hoverableObjectDescription);
    }
    private void OnMouseExit()
    {
        if(hoverableObject.Initiated)
            hoverableObject.OnMouseExit();
    }
    private void OnDestroy()
    {
        if(hoverableObject.Initiated)
            hoverableObject.OnDestroy();
    }
    public override void CollisionRequest(HitInfo collision, ref bool collectStatus)
    {
        collectStatus = false;
        Creature entity = collision.entity.GetComponent<Creature>();
        
        if(entity != null && entity.InventorySystem != null && entity.InventorySystem.ContainsRepository(TargetRepository))
        {
            collectStatus = AddItem((InventoryRepository<Item>)entity.InventorySystem.GetRepository(TargetRepository), CollectableObject);
        }
        SendCollectRequest(collision, collectStatus);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < CollisionHandler.Forces.Count; i++)
        {
            PayKnock knock = CollisionHandler.Forces[i];
            knock.CurrentSpeed = Vector2.Reflect(knock.CurrentSpeed, collision.GetContact(0).normal);
            CollisionHandler.Forces[i] = knock;
        }
    }
    protected bool AddItem(InventoryRepository<Item> repository, Item item) => repository.AddItem(item);
}