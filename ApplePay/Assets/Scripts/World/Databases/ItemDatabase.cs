[System.Serializable]
public struct WeaponInfo
{
    public float AttackCooldown;
    public float AdditionalFacingTime;
    internal float timeSinceUse;
    internal bool OnCooldown;
    internal bool isActivatable;
    public void SetCooldown()
    {
        OnCooldown = true;
        timeSinceUse = 0.0f;
    }
}


[System.Serializable]
public struct WeaponAnimationSettings
{
    public float AnimationTime;
    public float Velocity;
    public float AngularVelocity;
    internal bool inAnimation;
}