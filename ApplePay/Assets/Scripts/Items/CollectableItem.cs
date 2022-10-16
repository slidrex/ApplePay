using UnityEngine;

public abstract class CollectableItem : CollectableObject
{
    protected abstract Item CollectableObject { get; }
    public abstract string TargetRepository { get; }
    [SerializeField] private ItemHoverableObject itemHoverableObject;

    public override void Collect(Collider2D collision, ref bool collectStatus)
    {
        OnCollectItem(collision, CollectableObject, out collectStatus);
        if(collectStatus == true)
        {
            OnCollect();
            GameObject obj = null;
            if(itemHoverableObject != null && itemHoverableObject.GetCurrentPanel() != null)
            {
                obj = itemHoverableObject.GetCurrentPanel().gameObject;;
            }
            if(obj != null) Destroy(obj);
        }

    }
    
    protected virtual void OnCollectItem(Collider2D collectorCollider, Item item, out bool pickStatus)
    {
        pickStatus = false;
        InventorySystem inventorySystem = collectorCollider.GetComponent<InventorySystem>();
        if(inventorySystem == null)
            return;
        
        if(inventorySystem.ContainsRepository(TargetRepository) == false) 
        {
            Debug.LogWarning(inventorySystem + " system doesn't contain \"" +  TargetRepository + "\" repository");
            return;
        }
        pickStatus = AddItem(collectorCollider.GetComponent<InventorySystem>(), TargetRepository, CollectableObject);
        
    }
    protected bool AddItem(InventorySystem system, string repository, Item item)
    {
        if(system != null) return system.GetRepository(repository).AddItem(item);
        return false;
    }
}
