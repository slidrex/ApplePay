using UnityEngine;
[CreateAssetMenu(menuName = "Item/Weapon/Animation Preset", fileName = "new animation preset")]
public class WeaponAnimation : ScriptableObject
{
    public AnimationCurve VelocityPattern;
    public AnimationCurve AngularVelocityPattern;
    public bool RandomAngularVelocityDirection;
}