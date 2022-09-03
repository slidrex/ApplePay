using UnityEngine;

public abstract class CollectableItem : CollectableObject
{
    protected abstract Item CollectableObject { get; }
    [SerializeField] private ItemHoverableObject itemHoverableObject;

    public override void Collect(Collider2D collision, ref bool collectStatus)
    {
        base.Collect(collision, ref collectStatus);
        OnCollect(collision, CollectableObject, out collectStatus);
        GameObject obj = null;
        if(itemHoverableObject != null && itemHoverableObject.GetCurrentPanel() != null)
        {
            obj = itemHoverableObject.GetCurrentPanel().gameObject;;
        }
        if(obj != null) Destroy(obj);
    }
    protected virtual void OnCollect(Collider2D collectorCollider, Item item, out bool pickStatus)
    {
        pickStatus = false;
        InventorySystem inventorySystem = collectorCollider.GetComponent<InventorySystem>();
        if(inventorySystem == null)
            return;
        
        inventorySystem.AddItem(item, out pickStatus);
    }
}
