using UnityEngine;

public class StraightProjectileWeapon : RangeWeapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        projectile = Instantiate(FireObject.gameObject, GetFirePointPos(), Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition))).GetComponent<Projectile>();
        projectile.Setup((attackPosition - originPosition).normalized, attacker, target);
        
        Physics2D.IgnoreCollision(Owner.gameObject.GetComponent<Collider2D>(), projectile.gameObject.GetComponent<Collider2D>());
        
    }
}
