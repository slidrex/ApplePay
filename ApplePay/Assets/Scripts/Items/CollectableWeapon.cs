public class CollectableWeapon : CollectableItem<CollectableWeapon>
{
    protected override string TargetRepository { get => "weapon"; }
    public override CollectableWeapon CollectableObject {get => this; }
    public Weapon weapon;
    protected override void Awake()
    {
        weapon = Instantiate(weapon);
        base.Awake();
    }
}