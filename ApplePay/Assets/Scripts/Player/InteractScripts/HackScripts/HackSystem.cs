using UnityEngine;

public abstract class HackSystem : InteractiveObject
{
    [Header("Hack Settings")]
    [HideInInspector] public float CurrentProgress;
    public float MaxProgress;
    public bool isUnlocked {get; protected set;}
    protected virtual void OnAfterHack(InteractManager interactEntity) => OnUnlock();
    public virtual void OnUnlock() => ChangeLockStatus(false);
    public virtual void ChangeLockStatus(bool isLocked) => isUnlocked = isLocked;
}