public class CollectableWeapon : CollectableItem<WeaponItem>
{
    protected override string TargetRepository { get => "weapon"; }
    protected override void Awake()
    {
        base.Awake();
        CollectableObject.DropPrefab = this;
    }
    [field: UnityEngine.SerializeField] protected override WeaponItem CollectableObject {get;set;}
}