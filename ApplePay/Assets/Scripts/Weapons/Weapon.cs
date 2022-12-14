using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon/Weapon object", fileName = "new weapon")]
public class Weapon : ScriptableObject
{
    public WeaponObject WeaponObject;
    public WeaponAttackAnimation AttackAnimationSettings;
    public WeaponInfo WeaponInfo;
    public virtual void Activate(Creature attacker, Vector2 endTrajectory, out GameObject weaponObject, Transform target, out Projectile projectile) => Activate(attacker, attacker.gameObject.transform.position, endTrajectory, out weaponObject, target, out projectile);
    public virtual void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, out GameObject weaponObject, Transform target, out Projectile projectile) 
    {
        WeaponObject _weaponObject = Instantiate(WeaponObject.gameObject, originPosition, Quaternion.identity).GetComponent<WeaponObject>();
        _weaponObject.Activate(attacker, originPosition, attackPosition, target, out projectile);
        weaponObject = _weaponObject.gameObject;
    }
}
public enum AttackType
{
    AllDir,
    CrossDir
}