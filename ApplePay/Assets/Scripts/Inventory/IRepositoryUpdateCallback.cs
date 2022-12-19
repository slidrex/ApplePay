public interface IRepositoryUpdateCallback<ItemType>
{
    public void OnBeforeRepositoryUpdate(InventoryRepository.UpdateType type, ref ItemType item); 
}
