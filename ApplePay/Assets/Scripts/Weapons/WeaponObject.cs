using UnityEngine;
public abstract class WeaponObject : MonoBehaviour, ICollideDamageDealer, IHitResponder
{
    [Header("Collide settings")]
    public PayHitShape Collider;
    private void Awake() => Collider.AddResponder(this);
    [SerializeField] private int collideDamage;
    public int CollideDamage {get => collideDamage; set => collideDamage = value;}
    public float Knockback;
    [Header("Weapon settings")]
    protected Creature Owner;
    public DamageType DamageType;
    
    protected virtual Vector2 GetCollisionNormal(Collider2D collider) => collider.ClosestPoint(transform.position) - (Vector2)transform.position;
    protected virtual void OnColliderHit(Collider2D collider) {}
    protected virtual void OnMissColliderHit(Collider2D collider) {}
    protected virtual void OnEntityHitEnter(Collider2D collider, Entity hitEntity)
    {
        if(hitEntity != Owner) DealCollideDamage(hitEntity, CollideDamage, Owner);
    }
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity?.Damage(damage, DamageType, dealer);
    public void LinkAttacker(Creature attacker) => Owner = attacker;
    public virtual void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile) => projectile = null;
    protected virtual void Update()
    {
        Collider.CheckHit();
    }
    public virtual void OnHitDetected(HitInfo hitInfo)
    {
        OnColliderHit(hitInfo.collider);
        if(hitInfo.entity != Owner)
        {
            OnEntityHitEnter(hitInfo.collider, hitInfo.entity);
            Collider.IgnoreShapes.Add(hitInfo.collider, 1f);
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