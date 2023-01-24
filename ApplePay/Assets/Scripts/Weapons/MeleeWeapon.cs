using UnityEngine;

public class MeleeWeapon : WeaponObject, ICollideDamageDealer, IHitResponder
{
    [Header("Collide settings")]
    public PayHitShape Collider;
    private void Awake() => Collider?.AddResponder(this);
    [SerializeField] private int collideDamage;
    public int CollideDamage {get => collideDamage; set => collideDamage = value;}
    public float Knockback;
    public float KnockbackTime;
    public DamageType DamageType;
    protected override GameObject[] OnActivate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target)
    {
        LinkAttacker(attacker);
        return null;
    }
    protected virtual Vector2 GetCollisionNormal(Collider2D collider) => collider.ClosestPoint(transform.position) - (Vector2)transform.position;
    protected virtual void OnColliderHit(Collider2D collider) {}
    protected virtual void OnMissColliderHit(Collider2D collider) {}
    protected virtual void OnEntityHitEnter(Collider2D collider, Entity hitEntity)
    {
        if(hitEntity != Owner) DealCollideDamage(hitEntity, CollideDamage, Owner);
            hitEntity.ForceHandler?.Knock(transform.up.normalized * Knockback, Knockback, KnockbackTime);
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity?.Damage(damage, DamageType, dealer);
    protected override void Update()
    {
        Collider.CheckHit();
    }    
    public virtual void OnHitDetected(HitInfo hitInfo)
    {
        OnColliderHit(hitInfo.collider);
        if(hitInfo.entity != Owner)
        {
            OnEntityHitEnter(hitInfo.collider, hitInfo.entity);
            Collider.IgnoreShape(hitInfo.entity.HitShape);
        }
        else
            OnMissColliderHit(hitInfo.collider);
    }

    public enum AttackSide
    {
        Attacker,
        Attacked
    }
}
