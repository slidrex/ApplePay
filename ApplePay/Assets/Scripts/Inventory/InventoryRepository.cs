using System.Linq;
[System.Serializable]

public abstract class InventoryRepository : UnityEngine.MonoBehaviour 
{
    public enum UpdateType
    {
        Add,
        Remove
    }
    public abstract string Id {get;}
    public InventorySystem AttachedSystem { get; set; }
}
public abstract class InventoryRepository<ItemType> : InventoryRepository
{
    public delegate void BeforeRepositoryUpdateCallback(UpdateType updateType, ref ItemType actionItem);
    public BeforeRepositoryUpdateCallback RepositoryUpdateCallback;
    public UnityEngine.Transform itemInstancesContainer;
    public byte Capacity;
    public ItemType[] Items;
    private void Awake() => Items = new ItemType[Capacity];
    public ItemType[] GetExistingItems() => Items.Where(x => x != null).ToArray();
    public bool AddItem(ItemType item)
    {
        SendUpdateCallback(InventoryRepository.UpdateType.Add, ref item);
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i] == null)
            {
                Items[i] = item;
                OnItemAdded(item, i);
                return true;
            }
        }
        UnityEngine.Debug.LogWarning("Item hasn't been added!");
        return false;
    }
    private void SendUpdateCallback(UpdateType type, ref ItemType actionItem) => RepositoryUpdateCallback?.Invoke(type, ref actionItem);
    public virtual void OnItemAdded(ItemType item, int index) { }
    ///<summary>Returns true if inventory can add new item.</summary>
    public bool IsValid() => !IsFull();
    public bool IsFull()
    {
        foreach(ItemType item in Items)
        {
            if(item == null) return false;
        }
        return true;
    }
    protected void SaveRepositoryObjectInstance(UnityEngine.GameObject instance)
    {
        UnityEngine.GameObject gameObject = Instantiate(instance);
        gameObject.SetActive(false);
        gameObject.transform.position = itemInstancesContainer.transform.position;
        gameObject.transform.SetParent(itemInstancesContainer);
    }
    public bool RemoveItem(ItemType item)
    {
        SendUpdateCallback(InventoryRepository.UpdateType.Remove, ref item);
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i].Equals(item)) 
            {
                OnItemRemoved(item);
                Items[i] = default(ItemType);
                return true;
            }
            else if(i == Items.Length) UnityEngine.Debug.LogWarning("Repository you are trying to remove item from doesn't contain specified item!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been removed!");
        return false;
    }
    public virtual void OnItemRemoved(ItemType item) { }
}