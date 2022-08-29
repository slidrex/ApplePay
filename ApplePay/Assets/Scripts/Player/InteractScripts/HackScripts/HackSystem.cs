using UnityEngine;

public abstract class HackSystem : InteractiveObject
{
    [Header("Hack Settings")]
    [HideInInspector] public float CurrentProgess;
    public float MaxProgress;
    public bool isUnlocked {get; protected set;}
    protected virtual void OnAfterHack() => OnUnlock();
    protected void AfterHack() => OnAfterHack();
    public virtual void OnUnlock()  => ChangeLockStatus(false);
    public virtual void ChangeLockStatus(bool isLocked) => isUnlocked = isLocked;
}