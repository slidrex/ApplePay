using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon/Single Projectile", fileName = "new weapon")]

public class StraightProjectileWeapon : Weapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, out GameObject weaponObject, Transform target, out Projectile projectile)
    {
        projectile = Instantiate(FireObject, originPosition, Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition)));
        weaponObject = FireObject.gameObject;
        Vector2 projectileMoveVector = (attackPosition - originPosition).normalized;
        
        projectile.Setup(projectileMoveVector, attacker, target);
        
        attacker.HitShape.IgnoreShape(projectile.HitBox);
        
    }
}
