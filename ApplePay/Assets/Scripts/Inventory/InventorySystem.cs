using UnityEngine;
using System.Collections.Generic;
public class InventorySystem : MonoBehaviour
{
    public Creature InventoryOwner;
    public List<InventoryRepository> Repositories = new List<InventoryRepository>();
    public Dictionary<ItemEffect, byte> ItemEffectBuffer = new Dictionary<ItemEffect, byte>();
    public void AddItem(Item item, out bool isAdded) 
    {
        isAdded = false;
        
        GetRepository(item.GetType())?.AddItem(item, out isAdded);
    }
    public InventoryRepository GetRepository(System.Type repositoryType)
    {
        UpdateRepositories();
        foreach(InventoryRepository repository in Repositories)
        {
            if(repository.RepositoryType == repositoryType)
                return repository;
        }
        Debug.LogWarning("Repository of type " + "\"" + repositoryType + "\"" + " hasn't been found.");
        return null;
    }
    private void UpdateRepositories()
    {
        CheckRepositories();
        LinkRepositories();
    }
    private void LinkRepositories()
    {
        foreach(InventoryRepository repository in Repositories)
        {
            if(repository.AttachedSystem != this)
                repository.LinkInventorySystem(this);
        }
    }
    private void CheckRepositories()
    {
        for(int i = 0; i < Repositories.Count; i++)
        {
            for(int j = i + 1; j < Repositories.Count; j++)
            {
                if(Repositories[j].GetType() == Repositories[i].GetType())
                    throw new System.Exception(this + " inventory system contains multiple " + Repositories[j] + " repositories.");
            }
        }
    }
}