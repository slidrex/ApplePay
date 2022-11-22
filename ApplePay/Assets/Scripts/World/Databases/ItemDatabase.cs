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
    public ChangeManual[] Manuals;
    public CharmObject Item;
    private bool initiated;
    public CharmType Type {get 
    {
        CharmObject charm = Item as Charm;
        if(charm == null)
        {
            return CharmType.Switchable;
        }
        return CharmType.Base;
    }
    }
    public byte ActiveIndex { get; set; }
    public byte CharmsCount {get 
    {
        if(Type == CharmType.Switchable) return (byte)((MixedCharm)Item).Charms.Length;
        return 1;
    }}
    private void Init()
    {
        int charmCount = CharmsCount;
        Manuals = new ChangeManual[charmCount];
        for(int i = 0; i < charmCount; i++)
        {
            Charm curCharm = GetCharm(i);
            Manuals[i] = new ChangeManual();
            for(int j = 0; j < curCharm.Attributes.Length; j++)
            {
                Manuals[i].attributeFields.Add(j, new VirtualBase(curCharm.Attributes[j].AdditionalAttributeValue));
            }
            for(int j = 0; j < curCharm.CharmFields.Length; j++)
            {
                Manuals[i].additionalFields.Add(curCharm.CharmFields[j].name, new VirtualBase(curCharm.CharmFields[j].value));
            }
        }
        initiated = true;
    }
    public Charm GetCharm(int index)
    {
        if(Type == CharmType.Base && index == 0) return GetActiveCharm();
        
        if(Type == CharmType.Switchable) 
        {
            MixedCharm charm = (MixedCharm)Item;
            return charm.Charms[index];
        }
        return null;
    }
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
        if(initiated == false) Init();
        if(Type == CharmType.Switchable)
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].BeginFunction(system.InventoryOwner, Manuals[ActiveIndex]);
        }
        else
        {
            Charm charm = (Charm)Item;
            charm.BeginFunction(system.InventoryOwner, Manuals[ActiveIndex]);
        }
            
    }
    public override void OnRepositoryRemoved(InventorySystem system) 
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.EndFunction(system.InventoryOwner, Manuals[ActiveIndex]);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].EndFunction(system.InventoryOwner, Manuals[ActiveIndex]);
        }

    }
    public override void OnRepositoryUpdate(InventorySystem system)
    {
        if(Type == CharmType.Base)
        {
            Charm charm = (Charm)Item;
            charm.UpdateFunction(system.InventoryOwner, Manuals[ActiveIndex]);
        }
        else
        {
            MixedCharm charm = (MixedCharm)Item;
            charm.Charms[ActiveIndex].UpdateFunction(system.InventoryOwner, Manuals[ActiveIndex]);
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
    public System.Collections.Generic.Dictionary<int, VirtualBase> attributeFields = new System.Collections.Generic.Dictionary<int, VirtualBase>();
    public System.Collections.Generic.Dictionary<string, VirtualBase> additionalFields = new System.Collections.Generic.Dictionary<string, VirtualBase>();
    public System.Collections.Generic.List<EntityAttribute.TagAttribute> TagAttributeCache = new System.Collections.Generic.List<EntityAttribute.TagAttribute>();
}