///<summary>
///Provides value that can be modified by tagged modifiers.
///</summary>
public struct VirtualBase
{
    public float SourceValue;

    public System.Collections.Generic.List<BaseValue> BaseModifiers {get; private set;}
    public VirtualBase(float sourceValue)
    {
        SourceValue = sourceValue;
        BaseModifiers = new System.Collections.Generic.List<BaseValue>();
    }
    public BaseValue AddValue(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(value), GetStaticFloatRef(1f), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddValue(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, value, GetStaticFloatRef(1f), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddPercent(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), GetStaticFloatRef(1 + value), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddPercent(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), new VirtualFloatRef(() => 1 + value.Get()), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddMultiplier(float value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(1f), GetStaticFloatRef(value), tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    public BaseValue AddMultiplier(VirtualFloatRef value, params string[] tags)
    {
        BaseValue baseValue = new BaseValue(this, GetStaticFloatRef(0), value, tags);
        
        BaseModifiers.Add(baseValue);
        return baseValue;
    }
    ///<summary>
    ///Returns the value modified by all Virtual Base Modifiers
    ///</summary>
    public float GetValue()
    {
        double value = SourceValue;
        foreach(BaseValue baseValue in BaseModifiers)
        {
            value += baseValue.AdditionalValue.Get();
        }
        foreach(BaseValue baseValue in BaseModifiers)
        {
            value *= baseValue.Multiplier.Get();
        }
        
        return (float)value;
    }
    private static VirtualFloatRef GetStaticFloatRef(float value) => new VirtualFloatRef(() => value);
    [System.Serializable]
    public struct BaseValue
    {
        internal VirtualBase m_base;
        public System.Collections.Generic.List<string> Tags;
        public System.Collections.Generic.List<DestroyClock> DestroyClocks;
        public VirtualFloatRef Multiplier;
        public VirtualFloatRef AdditionalValue;
        public BaseValue(VirtualBase virtualBase, VirtualFloatRef value, VirtualFloatRef multiplier, string[] tags)
        {
            m_base = virtualBase;
            AdditionalValue = value;
            Multiplier = multiplier;
            Tags = new System.Collections.Generic.List<string>();
            DestroyClocks = new System.Collections.Generic.List<DestroyClock>();
            Tags.AddRange(tags);
        }
        public struct DestroyClock
        {
            internal BaseValue BaseValue;
            internal System.Collections.IEnumerator Coroutine;
            public void Remove() 
            {
                BaseValue.DestroyClocks.Remove(this);
                StaticCoroutine.EndCoroutine(Coroutine);
            }
        }
        public void Remove() => m_base.BaseModifiers.Remove(this);
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
    public static VirtualBase.BaseValue.DestroyClock SetDestroyClock(this VirtualBase.BaseValue baseValue, float time)
    {
        VirtualBase.BaseValue.DestroyClock clock = new VirtualBase.BaseValue.DestroyClock();
        System.Collections.IEnumerator coroutine = DestroyTagAttribute(clock, time);
        clock.Coroutine = coroutine;
        clock.BaseValue = baseValue;
        StaticCoroutine.BeginCoroutine(coroutine);
        
        baseValue.DestroyClocks.Add(clock);
        return clock;
    }
    private static System.Collections.IEnumerator DestroyTagAttribute(VirtualBase.BaseValue.DestroyClock clock, float time)
    {
        yield return new UnityEngine.WaitForSeconds(time);
        clock.BaseValue.DestroyClocks.Remove(clock);
        clock.BaseValue.Remove();
    }
}