///<summary>
///Provides value that can be modified by tagged modifiers.
///</summary>
public struct VirtualBase
{
    public float SourceValue;
    public System.Collections.Generic.List<BaseValue> BaseModifiers { get; private set; }
    
    public VirtualBase(float sourceValue)
    {
        SourceValue = sourceValue;
        BaseModifiers = new System.Collections.Generic.List<BaseValue>();
    }
    public BaseValue AddValue(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(value), GetStaticFloatRef(1f), GetStaticFloatRef(0), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddValue(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, value, GetStaticFloatRef(1f), GetStaticFloatRef(0), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddPercent(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), GetStaticFloatRef(1f), new VirtualFloatRef(() => value), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddPercent(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), GetStaticFloatRef(1f), new VirtualFloatRef(() => value.Get()), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddMultiplier(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0f), GetStaticFloatRef(value), GetStaticFloatRef(0), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddMultiplier(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), value, GetStaticFloatRef(0), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    ///<summary>
    ///Returns the value modified by all Virtual Base Modifiers
    ///</summary>
    public float GetValue()
    {
        float value = SourceValue;
        float resultMultilplier = 1f;
        foreach(BaseValue baseValue in BaseModifiers)
        {
            value += baseValue.AdditionalValue.Get();
        }
        foreach(BaseValue baseValue in BaseModifiers)
        {
            value *= baseValue.Multiplier.Get();
            resultMultilplier += baseValue.AdditionalSourcePercent.Get();
        }
        value *= resultMultilplier;
        return value;
    }
    private static VirtualFloatRef GetStaticFloatRef(float value) => new VirtualFloatRef(() => value);
    [System.Serializable]
    public struct BaseValue
    {
        internal VirtualBase m_base;
        public System.Collections.Generic.List<string> Tags;
        public System.Collections.Generic.List<Pay.Functions.Generic.ActionClock> DestroyClocks;
        public VirtualFloatRef Multiplier;
        public VirtualFloatRef AdditionalValue;
        public VirtualFloatRef AdditionalSourcePercent;
        public BaseValue(VirtualBase virtualBase, VirtualFloatRef value, VirtualFloatRef multiplier, VirtualFloatRef sourcePercent, string[] tags)
        {
            m_base = virtualBase;
            AdditionalValue = value;
            Multiplier = multiplier;
            AdditionalSourcePercent = sourcePercent;
            Tags = new System.Collections.Generic.List<string>();
            DestroyClocks = new System.Collections.Generic.List<Pay.Functions.Generic.ActionClock>();
            Tags.AddRange(tags);
        }
        public void Remove() => m_base.BaseModifiers.Remove(this);
        public struct DestroyClock
        {
            public Pay.Functions.Generic.ActionClock Clock;
        }
    }
    ///<summary>
    ///Readonly referenced value.
    ///</summary>
    public struct VirtualFloatRef
    {
        public System.Func<float> Get;
        public VirtualFloatRef(System.Func<float> getter)
        {
            Get = getter;
        }
    }
}
public static class VirtualBaseExtension
{
    public static bool ContainsModifiedTag(this VirtualBase virtualBase, string tag)
    {
        foreach(VirtualBase.BaseValue baseValue in virtualBase.BaseModifiers)
        {
            if(baseValue.Tags.Contains(tag)) return true;
        }
        return false;
    }
}
public static class DestroyClockExtension
{
    ///<summary>Sets the tagged attribute termination time.</summary>
    public static Pay.Functions.Generic.ActionClock SetDestroyClock(this VirtualBase.BaseValue baseValue, float time)
    {
        Pay.Functions.Generic.ActionClock clock = new Pay.Functions.Generic.ActionClock
        (
            delegate
            {
                baseValue.Remove();
            },
            time
        );
        clock.SetOnRemoveAction(() => baseValue.DestroyClocks.Remove(clock));
        baseValue.DestroyClocks.Add(clock);
        return clock;
    }
}