[System.Serializable]
public class WeaponItem
{
    public Weapon Weapon;
    public CollectableObject DropPrefab;
    public WeaponInfo WeaponInfo;
}
[System.Serializable]
public struct WeaponInfo
{
    public WeaponDisplay Display;
    public WeaponAnimationSettings AnimationParameters;
    public WeaponAnimationInfo AnimationInfo;
    public float GetAnimationTime() => AnimationParameters.AnimationTime;
    public float GetAttackInterval() => AnimationParameters.AttackInterval;
    public float GetVelocity() => AnimationParameters.VelocityMultiplier;
    public float GetAngularVelocity() => AnimationParameters.AngularVelocityMultiplier;
}
public struct WeaponAnimationInfo
{
    internal float timeSinceUse;
    internal bool inAnimation;
    internal bool canActivate;
}


[System.Serializable]
public class WeaponAnimationSettings
{
    public float AttackInterval = 0.5f;
    public float AnimationTime = 1f;
    public float VelocityMultiplier = 1;
    public float AngularVelocityMultiplier = 1;
}