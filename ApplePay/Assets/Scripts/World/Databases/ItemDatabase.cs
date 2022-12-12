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
public struct WeaponAnimationSettings
{
    public float AttackInterval;
    public float AnimationTime;
    public float VelocityMultiplier;
    public float AngularVelocityMultiplier;
}