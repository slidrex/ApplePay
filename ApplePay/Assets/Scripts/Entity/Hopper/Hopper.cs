using UnityEngine;
public class Hopper : AttackingMob, ICollideDamageDealer
{
    private SimplifiedWeaponHolder holder;
    private Animator anim;
    private MobMovement movement;
    [SerializeField] private float seenRadius;
    private const float hitIgnoreCollisionTime = 0.6f;
    public int CollideDamage {get; set;} = 15;
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        holder = GetComponent<SimplifiedWeaponHolder>();
        movement = GetComponent<MobMovement>();
    }
    public override void OnHitDetected(HitInfo hitInfo)
    {
        Creature entity = hitInfo.entity as Creature;
        if(entity != null && PayTagHandler.IsHostile(this, entity))
        {
            DealCollideDamage(hitInfo.entity, CollideDamage, this);
            hitInfo.entity.HitShape.IgnoreShape(HitShape, hitIgnoreCollisionTime);
        }
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity?.Damage(damage, DamageType.Physical, dealer);
}