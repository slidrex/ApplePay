using UnityEngine;
public abstract class WeaponObject : MonoBehaviour, ICollideDamageDealer
{
    protected Creature Owner;
    [SerializeField] private int collideDamage;
    public int CollideDamage {get => collideDamage; set => collideDamage = value;}
    public float Knockback;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnColliderHit(collider);
        if(collider.GetComponent<Entity>() != null && !collider.gameObject.GetComponent<Entity>().Equals(Owner))
        {
            OnEntityHitEnter(collider, collider.gameObject.GetComponent<Entity>());
        }
        else
            OnMissColliderHit(collider);
        
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Rigidbody2D>() != null) collider.gameObject.GetComponent<Rigidbody2D>()?.AddForce(GetCollisionNormal(collider) * Knockback);
        if(collider.gameObject.GetComponent<Entity>() != null) OnEntityHitStay(collider, collider.gameObject.GetComponent<Entity>());
    }
    protected virtual Vector2 GetCollisionNormal(Collider2D collider) => collider.ClosestPoint(transform.position) - (Vector2)transform.position;
    protected virtual void OnColliderHit(Collider2D collider) {}
    protected virtual void OnMissColliderHit(Collider2D collider) {}
    protected virtual void OnEntityHitEnter(Collider2D collider, Entity hitEntity)
    {
        if(hitEntity != Owner) DealCollideDamage(hitEntity, CollideDamage, Owner);
    }
    protected virtual void OnEntityHitStay(Collider2D collider, Entity entity) {}
    public void DealCollideDamage(Entity entity, int damage, Creature dealer) => entity?.ChangeHealth(-damage, dealer);
    public void LinkAttacker(Creature attacker) => Owner = attacker;
    public virtual void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile) => projectile = null;
}