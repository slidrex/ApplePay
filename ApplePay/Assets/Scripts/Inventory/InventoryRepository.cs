using System.Linq;
[System.Serializable]
public abstract class InventoryRepository : UnityEngine.MonoBehaviour 
{
    public abstract string Id {get;}
    public InventorySystem AttachedSystem { get; set; }
}
public abstract class InventoryRepository<ItemType> : InventoryRepository
{
    public byte Capacity;
    public ItemType[] Items;
    private void Awake() => Items = new ItemType[Capacity];
    public ItemType[] GetExistingItems() => Items.Where(x => x != null).ToArray();
    public virtual bool AddItem(ItemType item)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i] == null)
            {
                Items[i] = item;
                return true;
            }
        }
        UnityEngine.Debug.LogWarning("Item hasn't been added!");
        return false;
    }
    public bool IsFull()
    {
        foreach(ItemType item in Items)
        {
            if(item == null) return false;
        }
        return true;
    }
    public virtual bool RemoveItem(ItemType item)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i].Equals(item)) 
            {
                Items[i] = default(ItemType);
                return true;
            }
            else if(i == Items.Length) UnityEngine.Debug.LogWarning("Repository you are trying to remove item from doesn't contain specified item!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been removed!");
        return false;
    }
}