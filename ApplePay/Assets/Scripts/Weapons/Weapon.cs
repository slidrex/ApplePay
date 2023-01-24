using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Weapon.AttackType attackType;
    public WeaponDisplay display;
    public WeaponInfo weaponInfo;
    private Vector2 freezedAxises;
    protected virtual void Awake() => weaponInfo.SetCooldown();
    public Vector2 GetAttackVector(Vector2 originPosition, Vector2 attackPosition)
    {
        Vector2 distance = (attackPosition - originPosition);
        if(attackType == AttackType.Radial) return distance.normalized;
        else if(attackType == AttackType.Cross)
        {
            if(freezedAxises == Vector2.zero)
            {
                return Mathf.Abs(distance.x) > Mathf.Abs(distance.y) ? Vector2.right * Mathf.Sign(distance.x) : Vector2.up * Mathf.Sign(distance.y);
            }
            else if(freezedAxises == Vector2.right) return Vector2.up * Mathf.Sign(distance.y);
            else if(freezedAxises == Vector2.up) return Vector2.right * Mathf.Sign(distance.x);
        }
        return Vector2.zero;
    }
    ///<summary>Vector2.up - freeze Y; Vector2.right - freeze X;</summary>
    public void FreezeHorizontalAttackAxis()
    {
        freezedAxises = Vector2.right;
    }
    public void FreezeVerticalAttackAxis()
    {
        freezedAxises = Vector2.up;
    }
    public abstract void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out GameObject[] output);
    public enum AttackType
    {
        Radial,
        Cross
    }
}