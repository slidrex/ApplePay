using UnityEngine;
[System.Serializable]
public abstract class InventoryRepository : MonoBehaviour
{
    public RepositoryRenderer RepositoryRenderer { get; set; }
    public InventorySystem AttachedSystem {get; private set;}
    public byte Capacity;
    public abstract System.Type RepositoryType { get; }
    public System.Collections.Generic.List<Item> RepositoryItems {get; private set;} = new System.Collections.Generic.List<Item>();
    public void LinkInventorySystem(InventorySystem inventorySystem) => AttachedSystem = inventorySystem;
    public virtual void AddItem(Item item, out bool isAdded)
    {
        isAdded = false;
        if(RepositoryItems.Count >= Capacity)
        {
            OnCapacityLimit(ref item);
            return;
        }
        RepositoryItems.Add(item);
        isAdded = true;
        OnItemAdded(item);
        RepositoryRenderer?.OnRepositoryUpdate(this);
    }
    public virtual void AddItem(Item item, byte index, out bool isAdded)
    {
        isAdded = false;
        if(index > Capacity)
            return;
        RepositoryItems[index] = item;
        OnItemAdded(item, index);
        RepositoryRenderer?.OnRepositoryUpdate(this);
    }
    public void RemoveItem(Item item, out bool removed) 
    {
        removed = RepositoryItems.Remove(item);
        if(removed) 
        {
            OnItemRemoved(item);
            RepositoryRenderer?.OnRepositoryUpdate(this);
        }
    }
    protected virtual void Update() {}
    public virtual void OnItemAdded(Item added) {}
    public virtual void OnItemAdded(Item added, byte index) {}
    public virtual void OnCapacityLimit(ref Item item) {}
    public virtual void OnItemRemoved(Item removed) {}
}