using UnityEngine;

public class MeleeWeapon : WeaponObject
{
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        projectile = null;
    }
    protected override void OnEntityHitEnter(Collider2D collision, Entity hitEntity)
    {
        base.OnEntityHitEnter(collision, hitEntity);
        if(hitEntity.isKnockable == true)
        {
            hitEntity.CollisionHandler?.Knock(transform.up.normalized * Knockback, Knockback);
            
            Pay.Functions.Physics.IgnoreCollision(1f, Collider, collision);
        }
    }
}
