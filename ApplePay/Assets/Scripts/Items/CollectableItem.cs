using UnityEngine;

public abstract class CollectableItem<Item> : CollectableObject
{
    protected abstract Item CollectableObject { get; set; }
    protected abstract string TargetRepository { get; }
    [SerializeField] private ItemHoverableObject itemHoverableObject;
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
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < CollisionHandler.Forces.Count; i++)
        {
            PayKnock knock = CollisionHandler.Forces[i];
            knock.CurrentSpeed = Vector2.Reflect(knock.CurrentSpeed, collision.GetContact(0).normal);
            CollisionHandler.Forces[i] = knock;
        }
    }
    protected bool AddItem(InventoryRepository<Item> repository, Item item) => repository.AddItem(item);

    private void OnDestroy()
    {
        itemHoverableObject?.TerminatePanel();
    }
}