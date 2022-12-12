public class WeaponRepository : InventoryRepository<CollectableWeapon>
{
    public AdvancedWeaponHolder Holder;

    public override string Id => "weapon";

    public override bool AddItem(CollectableWeapon item)
    {
        int index = -1;
        for(int i = 0; i < Items.Length; i++)
        {
            if(Items[i] == null)
            {
                index = i;
                break;
            }
        }
        Holder.OnBeforeRepositoryUpdate();
        bool success = IsValid();
        if(success)
        {
            CollectableWeapon _item = Instantiate(item);
            base.AddItem(_item);
            Holder.OnAddItem(_item, (byte)index);
            _item.gameObject.SetActive(false);
            _item.transform.SetParent(itemInstancesContainer);

        }
        return success;
    }
    
}
