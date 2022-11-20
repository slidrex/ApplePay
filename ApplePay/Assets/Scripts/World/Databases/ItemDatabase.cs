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
    public ChangeManual Manual = new ChangeManual();
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
            charm.Charms[ActiveIndex].BeginFunction(system.InventoryOwner, Manual);
        }
        else
        {
            Charm charm = (Charm)Item;
            charm.BeginFunction(system.InventoryOwner, Manual);
        }
            
    }
    public override void OnRepositoryRemoved(InventorySystem system) 
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.EndFunction(system.InventoryOwner, Manual);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].EndFunction(system.InventoryOwner, Manual);
        }

    }
    public override void OnRepositoryUpdate(InventorySystem system)
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.UpdateFunction(system.InventoryOwner, Manual);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].UpdateFunction(system.InventoryOwner, Manual);
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
public class ChangeManual
{
    private System.Collections.Generic.Dictionary<string, float> manualMultipliers = new System.Collections.Generic.Dictionary<string, float>();
    ///<summary>
    ///Adds manual multiplier. If multiplier exists in manual, values will be added together.
    ///</summary>
    public bool TryGetMultiplier(string name, out float multiplier) => manualMultipliers.TryGetValue(name, out multiplier);
    public bool ContainsMultiplier(string tag) => manualMultipliers.ContainsKey(tag);
    public void AddMultiplier(string name, float value)
    {
        if(!manualMultipliers.ContainsKey(name))
        {
            manualMultipliers.Add(name, value);
        }else manualMultipliers[name] += value;
    }
    public TagAttribute[] TagAttributes;
}