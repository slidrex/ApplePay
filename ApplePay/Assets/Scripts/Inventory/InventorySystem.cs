using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct InventoryRepositoryObject
{
    public byte RepositoryLength;
    public string Name;
    public RepositoryRenderer Renderer;
}

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventoryRepositoryObject[] StartRepositories;
    public Creature InventoryOwner;
    public Dictionary<string, InventoryRepository> Repositories = new Dictionary<string, InventoryRepository>();
    private void Awake() => FillSetupRepositories();
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
        foreach(InventoryRepositoryObject repository in StartRepositories) AddRepository(repository.Name, repository.RepositoryLength, repository.Renderer);
    }
    public void AddRepository(string name, byte length, RepositoryRenderer renderer) => Repositories.Add(name, new InventoryRepository(this, length, renderer));
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