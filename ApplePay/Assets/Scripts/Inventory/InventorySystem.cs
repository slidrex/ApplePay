using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]

public struct InventoryRepositoryObject
{
    public byte RepositoryLength;
    public string Name;
    [UnityEngine.Tooltip("Accessor is a component that has a repository access interface."), UnityEngine.SerializeField] internal UnityEngine.MonoBehaviour[] _hanlders;
}

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventoryRepositoryObject[] StartRepositories;
    public Creature InventoryOwner;
    public Dictionary<string, InventoryRepository> Repositories = new Dictionary<string, InventoryRepository>();
    private void Awake()
    {
        InventoryOwner.InventorySystem = this;
        FillSetupRepositories();
    }
    private void Update()
    {
        UpdateRepositoryItems();
    }
    private void UpdateRepositoryItems()
    {
        foreach(InventoryRepository repository in Repositories.Values)
        {
            foreach(Item item in repository.Items)
            {
                item?.OnRepositoryUpdate(this);
            }
        }
    }
    private void FillSetupRepositories()
    {
        for(int i = 0; i < StartRepositories.Length; i++)
        {
            for(int j = 0; j < StartRepositories[i]._hanlders.Length; j++)
            {
                if(StartRepositories[i]._hanlders[j] == null) Debug.LogWarning("The script " + StartRepositories[i]._hanlders[j].name + " in" + StartRepositories[i].Name + "repository has no handlers specified.");

            }
            AddRepository(StartRepositories[i].Name, StartRepositories[i].RepositoryLength, StartRepositories[i]._hanlders.Select(x => x.GetComponent<IRepositoryHandler>()).ToArray());

        }
        
    }
    public void AddRepository(string name, byte length, params IRepositoryHandler[] handlers) => Repositories.Add(name, new InventoryRepository(this, length, handlers));
    public InventoryRepository GetRepository(string repositoryName)
    {
        InventoryRepository repository = new InventoryRepository();
        Repositories.TryGetValue(repositoryName, out repository);
        return repository;
    }
    public bool ContainsRepository(string repositoryName) => Repositories.ContainsKey(repositoryName);
    public bool AddItem(string repositoryName, Item item)
    {
        InventoryRepository repository = GetRepository(repositoryName);
        
        return repository.AddItem(item);
    }
    public void RemoveItem(string repositoryName, Item item)
    {
        InventoryRepository repository = GetRepository(repositoryName);

        repository.RemoveItem(item);
    }
}