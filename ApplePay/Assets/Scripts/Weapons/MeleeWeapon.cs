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
        if(hitEntity.gameObject.GetComponent<Rigidbody2D>() != null) hitEntity.GetComponent<Rigidbody2D>().AddForce(transform.up * Knockback, ForceMode2D.Impulse);
    }
}
