public abstract class Item  
{
    public virtual void OnRepositoryAdded(InventorySystem system) { }
    public virtual void OnRepositoryUpdate(InventorySystem system) { }
    public virtual void OnRepositoryRemoved(InventorySystem system) { }
}

[System.Serializable]
public class WeaponItem : Item
{
    public Weapon Weapon;
    public CollectableObject DropPrefab;
    public WeaponInfo WeaponInfo;
}
[System.Serializable]
public struct WeaponInfo
{
    public WeaponAnimationSettings AnimationParameters;
    public ItemDisplay Display;
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
public class CharmItem : Item
{
    public Charm Item;
    public override void OnRepositoryAdded(InventorySystem system) => Item.BeginFunction(system.InventoryOwner);
    public override void OnRepositoryRemoved(InventorySystem system) => Item.EndFunction(system.InventoryOwner);
    public override void OnRepositoryUpdate(InventorySystem system) => Item.UpdateFunction(system.InventoryOwner);
}
[System.Serializable]
public class WeaponAnimationSettings
{
    public float AttackInterval = 0.5f;
    public float AnimationTime = 1f;
    public float VelocityMultiplier = 1;
    public float AngularVelocityMultiplier = 1;
}
