public class WeaponRepository : InventoryRepository<CollectableWeapon>
{
    public AdvancedWeaponHolder Holder;

    public override string Id => "weapon";
    public override void OnItemAdded(CollectableWeapon item, int index)
    {
        Holder.OnAddItem(item, (byte)index);
        item.gameObject.SetActive(false);
        item.transform.SetParent(itemInstancesContainer);
    }
    
}
