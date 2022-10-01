[System.Serializable]

public struct InventoryRepository
{
    public RepositoryRenderer Renderer;
    public InventorySystem System;
    public Item[] Items;
    public InventoryRepository(InventorySystem owner, byte capacity, RepositoryRenderer renderer)
    {
        Items = new Item[capacity];
        System = owner;
        Renderer = renderer;
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
                repository.Renderer.OnRepositoryUpdate();
                return true;
            } else
            if(i == repository.Items.Length - 1) UnityEngine.Debug.LogWarning("Repository you are trying to add item to is full!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been added!");
        return false;
    }
    public static bool RemoveItem(this InventoryRepository repository, Item item)
    {
        for(int i = 0; i < repository.Items.Length; i++)
        {
            if(repository.Items[i] == item) 
            {
                repository.Items[i].OnRepositoryRemoved(repository.System);
                repository.Renderer?.OnRepositoryUpdate();
                repository.Items[i] = null;
                return true;
            }
            else if(i == repository.Items.Length) UnityEngine.Debug.LogWarning("Repository you are trying to remove item from doesn't contain specified item!");
        }
        UnityEngine.Debug.LogWarning("Item hasn't been removed!");
        return false;
    }
}
