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
            if(repository.Items[i] != null)
            {
                ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(repository.Items[i].weapon.display.Rarity);
                
                Slots[i].RenderIcon(repository.Items[i].weapon.display.Icon);
                Slots[i].RenderRarityFrame(rarityInfo.color);
            }
            else 
            {
                Slots[i].RenderRarityFrame(UnityEngine.Color.white, false);
                Slots[i].RenderIcon(null);
            }
        }
    }
}
