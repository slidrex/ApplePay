using System;
using UnityEngine;

public class WeaponRepositoryRenderer : DragRepositoryRenderer<CollectableWeapon>
{
    public override string RepositoryType => "weapon";
    protected override Action<int> RenderSlotCall => RenderSlot;
    private void OnEnable() => Render();
    private void RenderSlot(int index)
    {
        CollectableWeapon weapon = repository.Items[index];
        InventoryWeaponSlot slot = Slots[index] as InventoryWeaponSlot;
        if(weapon == null)
        {
            slot.RenderSlotFrame(Color.white, false);
            slot.LinkItem(null);
            slot.RenderIcon(null);
        }
        else
        {
            ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(repository.Items[index].weapon.display.Rarity);
                
            slot.RenderIcon(repository.Items[index].weapon.display.Icon);
            slot.LinkItem(repository.Items[index]);
            slot.RenderSlotFrame(rarityInfo.color);
        }
    }
}
