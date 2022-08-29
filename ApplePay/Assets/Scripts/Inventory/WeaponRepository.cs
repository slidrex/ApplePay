public class WeaponRepository : InventoryRepository
{
    [UnityEngine.SerializeField] private AdvancedWeaponHolder AttachedHolder;
    public override System.Type RepositoryType { get; } = typeof(WeaponItem);
    public System.Collections.Generic.List<WeaponItem> InventoryItems = new System.Collections.Generic.List<WeaponItem>();
    public override void OnItemAdded(Item added) 
    {
        WeaponItem weaponItem = (WeaponItem)added;
        InventoryItems.Add(weaponItem);
        AttachedHolder.OnAddItem(ref weaponItem);
    }
    public override void OnItemAdded(Item added, byte index)
    {
        if(index > Capacity) return;
        WeaponItem weaponItem = (WeaponItem)added;
        InventoryItems[index] = weaponItem;
        AttachedHolder.OnAddItem(ref weaponItem);
    }

    public override void OnCapacityLimit(ref Item item)
    {
        WeaponItem weaponItem = (WeaponItem)item;
        AttachedHolder.OnCapacityLimit(ref weaponItem);
    }
    public override void OnItemRemoved(Item removed) => InventoryItems.Remove((WeaponItem)removed);
}