using UnityEngine;

public class StraightProjectileWeapon : RangeWeapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        Projectile proj = Instantiate(FireObject.gameObject, GetFirePointPos(), Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition))).GetComponent<Projectile>();
        Physics2D.IgnoreCollision(attacker.GetComponent<Collider2D>(), proj.GetComponent<Collider2D>());
        proj.Setup((attackPosition - originPosition).normalized, attacker, target);
        projectile = proj;
    }
}
