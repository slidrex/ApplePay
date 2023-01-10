using UnityEngine;

public class RangeWeapon : WeaponObject
{
    [SerializeField] protected Projectile Projectile;
    [SerializeField] protected Transform FirePoint;

    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile)
    {
        projectile = Instantiate(Projectile, GetFirePointPos(), Quaternion.identity);
        Vector2 moveVector = (attackPosition - originPosition).normalized;
        projectile.Setup(moveVector, attacker, target);
        projectile.DisableOwnerCollisions();
    }
    public Vector2 GetFirePointPos()
    {
        if(FirePoint != null) return FirePoint.position;
        return (Vector2)transform.position;
    }
}
