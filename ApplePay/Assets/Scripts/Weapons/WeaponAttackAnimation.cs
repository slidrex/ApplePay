using UnityEngine;
[CreateAssetMenu(menuName = "Item/Weapon/Animation Preset", fileName = "new animation preset")]
public class WeaponAttackAnimation : ScriptableObject
{
    public AttackType AttackType;
    public AnimationCurve VelocityPattern;
    public AnimationCurve AngularVelocityPattern;
    public bool RandomAngularVelocityDirection;
}