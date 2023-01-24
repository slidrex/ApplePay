public class CollectableWeapon : CollectableItem<CollectableWeapon>
{
    protected override bool DestroyOnCollect => false;
    protected override string TargetRepository { get => "weapon"; }
    public override CollectableWeapon CollectableObject {get => this; }
    [UnityEngine.SerializeField] private Weapon weapon;
    public Weapon Weapon { get; set; }
    protected override void Awake()
    {
        Weapon = Instantiate(weapon);
        base.Awake();
    }
}