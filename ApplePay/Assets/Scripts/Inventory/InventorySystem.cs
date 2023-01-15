using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public float dropItemOffset;
    public float dropItemBlockTime;
    private void Awake()
    {
        SystemOwner.InventorySystem = this;
        foreach(InventoryRepository repository in Repositories)
        {
            repository.AttachedSystem = this;
        }
    }
    public Creature SystemOwner;
    public InventoryRepository[] Repositories;
    public bool ContainsRepository(string repositoryID)
    {
        foreach(InventoryRepository _repository in Repositories)
        {
            if(_repository.Id == repositoryID) return true;
        }
        return false;
    }
    public InventoryRepository GetRepository(string repositoryID)
    {
        foreach(InventoryRepository _repository in Repositories)
        {
            if(_repository.Id == repositoryID) return _repository;
        }
        return null;
    }
}
