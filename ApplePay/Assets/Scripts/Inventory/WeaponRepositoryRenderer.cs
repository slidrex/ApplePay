public class WeaponRepositoryRenderer : RepositoryRenderer<CollectableWeapon>
{
    public override string RepositoryType => "weapon";
    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable() => Render();
    private void Render()
    {
        for(int i = 0; i < repository.Items.Length; i++)
        {
            InventoryDisplaySlot<CollectableWeapon> slot = Slots[i];
            if(repository.Items[i] != null)
            {
                ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(repository.Items[i].weapon.display.Rarity);
                
                slot.RenderIcon(repository.Items[i].weapon.display.Icon);
                slot.RenderSlotFrame(rarityInfo.color);
            }
            else 
            {
                slot.RenderSlotFrame(UnityEngine.Color.white, false);
                slot.RenderIcon(null);
            }
        }
    }
}
