public class CollectableWeapon : CollectableItem
{
    public WeaponItem WeaponItem;
    protected override Item CollectableObject { get => WeaponItem; }
}