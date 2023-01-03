using UnityEngine;

public class StraightProjectileWeapon : RangeWeapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        LinkAttacker(attacker);
        projectile = Instantiate(FireObject, GetFirePointPos(), Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition)));
        
        Vector2 projectileMoveVector = (attackPosition - originPosition).normalized;
        
        projectile.Setup(projectileMoveVector, attacker, target);
        
        Owner.HitShape.IgnoreShape(projectile.HitBox);
        
    }
}
