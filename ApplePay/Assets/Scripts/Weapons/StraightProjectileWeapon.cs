using UnityEngine;

public class StraightProjectileWeapon : RangeWeapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        projectile = Instantiate(FireObject.gameObject, GetFirePointPos(), Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition))).GetComponent<Projectile>();
        
        Physics2D.IgnoreCollision(attacker.gameObject.GetComponent<Collider2D>(), projectile.gameObject.GetComponent<Collider2D>());
        print("collision ignored!" + "(" + " " + attacker.gameObject.GetComponent<Collider2D>() + " " + projectile.gameObject.GetComponent<Collider2D>() + ")");
        projectile.Setup((attackPosition - originPosition).normalized, attacker, target);
    }
}
