public class CollectableWeapon : CollectableItem<WeaponItem>
{
    protected override string TargetRepository { get => "weapon"; }
    [field: UnityEngine.SerializeField] protected override WeaponItem CollectableObject {get;set;}
    protected override void Awake()
    {
        base.Awake();
        CollectableObject.DropPrefab = this;
    }
}