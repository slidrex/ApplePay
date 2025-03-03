using UnityEngine;

public abstract class CollectableItem<Item> : CollectableObject
{
    [UnityEngine.SerializeField] protected DroppedItemHint hoverableObject;
    private const float hintCreatingDistance = 2.0f;
    private Transform hintTriggerObject;
    protected virtual string hoverableObjectHeader {get => null;}
    protected virtual string hoverableObjectDescription {get => null;}
    public abstract Item CollectableObject { get; }
    protected abstract string TargetRepository { get; }
    protected override void Start()
    {
        base.Start();
        hintTriggerObject = FindObjectOfType<PlayerEntity>().transform;
        if(hoverableObjectHeader != null && hoverableObjectDescription != null)
            hoverableObject.Init(transform);
    }
    protected override void Update()
    {
        bool isInside = Vector2.SqrMagnitude(hintTriggerObject.position - transform.position) <= hintCreatingDistance * hintCreatingDistance;
        if(isInside && hoverableObject.HintCreated == false)
        {
            hoverableObject.CreateHint(hoverableObjectHeader, hoverableObjectDescription);
        }
        else if(isInside == false && hoverableObject.HintCreated == true)
        {
            hoverableObject.DestroyHint();
        }
        base.Update();
    }
    protected override void OnCollect(HitInfo collision)
    {
        if(hoverableObject.HintCreated)
        {
            hoverableObject.DestroyHint();
        }
        base.OnCollect(collision);
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