using System.Linq;
[System.Serializable]

public struct InventoryRepository
{
    public IRepositoryHandler[] Handlers;
    public InventorySystem System;
    public Item[] Items;
    ///<summary>
    ///Gets only existing items in the repository.
    ///</summary>
    public Item[] GetExistingItems() => Items.Where(x => x != null).ToArray();
    public InventoryRepository(InventorySystem owner, byte capacity, params IRepositoryHandler[] handlers)
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
        for(int i = 0; i < repository.Handlers.Length; i++) 
        {
            if(repository.Handlers[i] is IRepositoryPreUpdateHandler)
            {
                ((IRepositoryPreUpdateHandler)repository.Handlers[i]).OnBeforeRepositoryUpdate();
            }
        }

        for(int i = 0; i < repository.Items.Length; i++)
        {
            if(repository.Items[i] == null)
            {
                repository.Items[i] = item;
                repository.Items[i].OnRepositoryAdded(repository.System);
            for(int j = 0; j < repository.Handlers.Length; j++) 
            {
                if(repository.Handlers[j] is IRepositoryCallbackHandler)
                {
                    ((IRepositoryCallbackHandler)repository.Handlers[j]).OnRepositoryUpdated(item, (byte)i, RepositoryChangeFeedback.Added);
                }
            }
                return true;
            }
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
                for(int j = 0; j < repository.Handlers.Length; j++) 
                {
                    if(repository.Handlers[j] is IRepositoryCallbackHandler)
                    {
                       ((IRepositoryCallbackHandler)repository.Handlers[i]).OnRepositoryUpdated(item, (byte)i, RepositoryChangeFeedback.Removed);
                    }
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
