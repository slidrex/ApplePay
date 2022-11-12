using UnityEngine;

public abstract class CollectableItem : CollectableObject
{
    protected abstract Item CollectableObject { get; }
    
    public abstract string TargetRepository { get; }

    [SerializeField] private ItemHoverableObject itemHoverableObject;

    public override void CollisionRequest(Collision2D collision, ref bool collectStatus)
    {
        InventorySystem inventorySystem = collision.gameObject.GetComponent<InventorySystem>();
        collectStatus = false;
        if(inventorySystem != null && inventorySystem.ContainsRepository(TargetRepository))
        {
            collectStatus = AddItem(inventorySystem, TargetRepository, CollectableObject);
        }
        SendCollectRequest(collision, collectStatus);
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        for(int i = 0; i < CollisionHandler.Forces.Count; i++)
        {
            PayKnock knock = CollisionHandler.Forces[i];
            knock.CurrentSpeed = Vector2.Reflect(knock.CurrentSpeed, collision.GetContact(0).normal);
            CollisionHandler.Forces[i] = knock;
        }
    }
    protected bool AddItem(InventorySystem system, string repository, Item item)
    {
        if(system != null) return system.GetRepository(repository).AddItem(item);
        return false;
    }

    private void OnDestroy()
    {
        itemHoverableObject?.TerminatePanel();
    }
}
