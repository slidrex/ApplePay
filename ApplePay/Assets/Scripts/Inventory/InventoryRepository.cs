[System.Serializable]

public struct InventoryRepository
{
    public IRepositoryUpdateHandler[] Handlers;
    public InventorySystem System;
    public Item[] Items;
    public InventoryRepository(InventorySystem owner, byte capacity, params IRepositoryUpdateHandler[] handlers)
    {
        Items = new Item[capacity];
        System = owner;
        Handlers = handlers;
    }
}
public static class InventoryRepositoryExtension
{
    public static bool AddItem(this InventoryRepository repository, Item item)
    {
        for(int i = 0; i < repository.Items.Length; i++)
        {
            if(repository.Items[i] == null)
            {
                repository.Items[i] = item;
                repository.Items[i].OnRepositoryAdded(repository.System);
                foreach(IRepositoryUpdateHandler handler in repository.Handlers)
                {
                    handler.OnRepositoryUpdated(item, (byte)i, RepositoryChangeFeedback.Added);
                }
                return true;
            } else
            if(i == repository.Items.Length - 1) UnityEngine.Debug.LogWarning("Repository you are trying to add item to is full!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been added!");
        return false;
    }
    public static bool IsRepositoryFull(this InventoryRepository repository)
    {
        foreach(Item item in repository.Items)
        {
            if(item == null) return false;
        }
        return true;
    }
    public static bool RemoveItem(this InventoryRepository repository, Item item)
    {
        for(int i = 0; i < repository.Items.Length; i++)
        {
            if(repository.Items[i] == item) 
            {
                repository.Items[i].OnRepositoryRemoved(repository.System);
                foreach(IRepositoryUpdateHandler handler in repository.Handlers)
                {
                    handler.OnRepositoryUpdated(item, (byte)i, RepositoryChangeFeedback.Removed);
                }
                repository.Items[i] = null;
                return true;
            }
            else if(i == repository.Items.Length) UnityEngine.Debug.LogWarning("Repository you are trying to remove item from doesn't contain specified item!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been removed!");
        return false;
    }
}
