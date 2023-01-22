using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon/Single Projectile", fileName = "new weapon")]

public class StraightProjectileWeapon : Weapon
{
    [SerializeField] protected Projectile FireObject;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out GameObject output)
    {
        Projectile projectile = Instantiate(FireObject, originPosition, Quaternion.Euler(0, 0, Vector2.Angle(originPosition, attackPosition)));
        output = FireObject.gameObject;
        Vector2 projectileMoveVector = GetAttackVector(originPosition, attackPosition);
        
        projectile.Setup(projectileMoveVector, attacker, target);
        
        attacker.HitShape.IgnoreShape(projectile.HitBox);
        
    }
}
