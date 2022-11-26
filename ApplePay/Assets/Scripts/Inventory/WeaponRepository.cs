using UnityEngine;

public class WeaponRepository : InventoryRepository<WeaponItem>
{
    public AdvancedWeaponHolder Holder;
    public override string Id => "weapon";
    public override bool AddItem(WeaponItem item)
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
        bool success = base.AddItem(item);
        if(success) Holder.OnAddItem(item, (byte)index);
        return success;
    }
    
}
