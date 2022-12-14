[System.Serializable]
public struct WeaponInfo
{
    public WeaponDisplay Display;
    public WeaponAnimationSettings AnimationParameters;
    public WeaponAnimationInfo AnimationInfo;
    public float GetAnimationTime() => AnimationParameters.AnimationTime;
    public float GetAttackCooldown() => AnimationParameters.AttackCooldown;
    public float GetVelocity() => AnimationParameters.Velocity;
    public float GetAngularVelocity() => AnimationParameters.AngularVelocity;
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
    public float AttackCooldown;
    public float AnimationTime;
    public float Velocity;
    public float AngularVelocity;
}