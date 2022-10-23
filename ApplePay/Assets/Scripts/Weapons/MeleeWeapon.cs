using UnityEngine;

public class MeleeWeapon : WeaponObject
{
    [SerializeField] private float selfKnockback;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        projectile = null;
    }
    protected override void OnEntityHitEnter(Collider2D collision, Entity hitEntity)
    {
        base.OnEntityHitEnter(collision, hitEntity);
        hitEntity.CollisionHandler?.AddForce(transform.up.normalized * Knockback);
    }
}
