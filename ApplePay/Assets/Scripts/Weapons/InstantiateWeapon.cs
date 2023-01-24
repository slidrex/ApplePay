using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon/Instantiatable Weapon")]
public class InstantiateWeapon : Weapon
{
    public WeaponObject WeaponObject;
    public WeaponAnimation animationPreset;
    public AnimationInfo animationInfo;
    public override void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out GameObject[] output)
    {
        WeaponObject _weaponObject = Instantiate(WeaponObject, originPosition, Quaternion.identity);
        _weaponObject.Activate(attacker, originPosition, attackPosition, target);
        output = new GameObject[1] { _weaponObject.gameObject };
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