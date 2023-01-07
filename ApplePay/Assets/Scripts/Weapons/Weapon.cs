using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Weapon.AttackType attackType;
    public WeaponDisplay display;
    public WeaponInfo weaponInfo;
    protected virtual void Awake() => weaponInfo.SetCooldown();
    public abstract void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out GameObject output);
    public enum AttackType
    {
        Radial,
        Cross
    }
}