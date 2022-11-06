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
