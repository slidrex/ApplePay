public class CollectableWeapon : CollectableItem
{
    public WeaponItem WeaponItem;
    public override string TargetRepository { get => "weapons"; }
    protected override void Awake() 
    {
        base.Awake();
        WeaponItem.DropPrefab = this;
    }
    protected override Item CollectableObject { get => WeaponItem; }
}