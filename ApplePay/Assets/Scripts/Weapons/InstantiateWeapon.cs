using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon/Instantiatable Weapon")]
public class InstantiateWeapon : Weapon
{
    public WeaponObject WeaponObject;
    public WeaponAnimation animationPreset;
    public AnimationInfo animationInfo;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, out GameObject weaponObject, Transform target, out Projectile projectile)
    {
        WeaponObject _weaponObject = Instantiate(WeaponObject, originPosition, Quaternion.identity);
        _weaponObject.Activate(attacker, originPosition, attackPosition, target, out projectile);
        weaponObject = _weaponObject.gameObject;
    }
    [System.Serializable]
    public struct AnimationInfo
    {
        public float animationTime;
        public float linearVelocity;
        public float angularVelocity;
        [HideInInspector] public bool inAnimation;
    }
}