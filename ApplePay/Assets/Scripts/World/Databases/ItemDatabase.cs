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
    public WeaponDisplay Display;
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
    public CharmObject Item;
    public CharmType Type {get 
    {
        try { CharmObject charm = (Charm)Item; }
        catch { return CharmType.Switchable;}
        return CharmType.Base;
    }
    }
    public byte ActiveIndex { get; set; }
    public Charm GetActiveCharm()
    {
        if(Type == CharmType.Base) return (Charm)Item;
        else 
        {
            MixedCharm charm = (MixedCharm)Item;
            return charm.Charms[ActiveIndex];
        }  
    }
    public override void OnRepositoryAdded(InventorySystem system) 
    {
        if(Type == CharmType.Switchable)
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].BeginFunction(system.InventoryOwner);
        }
        else
        {
            Charm charm = (Charm)Item;
            charm.BeginFunction(system.InventoryOwner);
        }
    }
    public override void OnRepositoryRemoved(InventorySystem system) 
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.EndFunction(system.InventoryOwner);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].EndFunction(system.InventoryOwner);
        }

    }
    public override void OnRepositoryUpdate(InventorySystem system)
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.UpdateFunction(system.InventoryOwner);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].UpdateFunction(system.InventoryOwner);
        }
    }
}
[System.Serializable]
public class WeaponAnimationSettings
{
    public float AttackInterval = 0.5f;
    public float AnimationTime = 1f;
    public float VelocityMultiplier = 1;
    public float AngularVelocityMultiplier = 1;
}
