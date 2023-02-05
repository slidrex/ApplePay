using System.Linq;

[System.Serializable]

public class EntityAttribute
{
    private VirtualBase virtualBase;
    internal FloatRef ReferencedAttribute;

    public EntityAttribute(FloatRef attribute, float baseAttributeValue)
    {
        ReferencedAttribute = attribute;
        virtualBase = new VirtualBase(baseAttributeValue);
    }
    public float GetSourceValue() => virtualBase.SourceValue;
    public float GetAttributeValue() => virtualBase.GetValue();

    public void ApplyResult() => ReferencedAttribute.Set(virtualBase.GetValue());

    public enum AttributeOperation { Add, Multiply }

    public enum AttributeType { Multiplier, BaseAttributeValue }
    [System.Serializable]
    public class TagAttribute
    {
        private VirtualBase.BaseValue value;
        private EntityAttribute attachedAttribute;
        public float GetValue() => value.AdditionalValue.Get();
        public float GetMultiplier() => value.Multiplier.Get();
        public void Remove()
        {
            value.Remove();
            attachedAttribute.ApplyResult();
        }
        public System.Collections.Generic.List<Pay.Functions.Generic.ActionClock> GetDestroyClocks() => value.DestroyClocks;
        public Pay.Functions.Generic.ActionClock SetDestroyClock(float time)
        {
            Pay.Functions.Generic.ActionClock actionClock = value.SetDestroyClock(time);
            actionClock.ReplaceAction(() => Remove());
            
            return actionClock;
        }
        public TagAttribute(VirtualBase.BaseValue baseValue, EntityAttribute attribute)
        {
            value = baseValue;
            attachedAttribute = attribute;
        }
    }
    public bool ContainsTagAttribute(string tag)
    {
        foreach(VirtualBase.BaseValue baseValue in virtualBase.BaseModifiers)
        {
            foreach(string _tag in baseValue.Tags)
            {
                if(_tag == tag) return true;
            }
        }
        return false;
    }
    public byte GetTagAttributesCount(string tag)
    {
        byte taggedAttributesCount = 0;
        foreach(VirtualBase.BaseValue tagAttribute in virtualBase.BaseModifiers)
        {
            foreach(string _tag in tagAttribute.Tags)
            {
                if(_tag == tag) 
                {
                    taggedAttributesCount++;
                    break;
                }
            }
        }
        return taggedAttributesCount;
    }
    public TagAttribute[] GetTagAttributes()
    {
        VirtualBase.BaseValue[] attribs = virtualBase.BaseModifiers.ToArray();
        TagAttribute[] attributes = new TagAttribute[attribs.Length];
        for(int i = 0; i < attributes.Length; i++) attributes[i] = new TagAttribute(attribs[i], this);
        return attributes;
    }
    public TagAttribute[] GetTagAttributes(string tag) 
    {
        VirtualBase.BaseValue[] attribs = virtualBase.BaseModifiers.Where(x => x.Tags.Contains(tag)).ToArray();
        TagAttribute[] attributes = new TagAttribute[attribs.Length];
        for(int i = 0; i < attributes.Length; i++) attributes[i] = new TagAttribute(attribs[i], this);
        return attributes;
    }
    public TagAttribute AddAttributeValue(float value, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddValue(value);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        
        ApplyResult();
        return attrib;
    }
    public TagAttribute AddAttributeValue(VirtualBase.VirtualFloatRef value, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddValue(value);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        
        ApplyResult();
        return attrib;
    }
    public TagAttribute AddPercent(float value, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddPercent(value, tags);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        ApplyResult();
        return attrib;
    }
    public TagAttribute AddPercent(VirtualBase.VirtualFloatRef value, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddPercent(value, tags);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        ApplyResult();
        return attrib;
    }
    public TagAttribute AddAttributeMultiplier(float multiplier, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddMultiplier(multiplier, tags);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        ApplyResult();
        return attrib;
    }
    public TagAttribute AddAttributeMultiplier(VirtualBase.VirtualFloatRef multiplier, params string[] tags)
    {
        VirtualBase.BaseValue baseValue = virtualBase.AddMultiplier(multiplier, tags);
        TagAttribute attrib = new TagAttribute(baseValue, this);
        ApplyResult();
        return attrib;
    }
}

public static class EntityAttributeExtension
{
    ///<summary>Turns variable into a result attribute.</summary>
    public static EntityAttribute AddAttribute(this Entity entity, string name, FloatRef attribute, float baseAttributeValue)
    {
        EntityAttribute attrib = new EntityAttribute(attribute, baseAttributeValue);
        entity.Attributes.Add(name, attrib);
        return attrib;
    }
    public static EntityAttribute FindAttribute(this Entity entity, string name) 
    {
        if(entity.Attributes.TryGetValue(name, out EntityAttribute attribute))
        {
            return attribute;
        }
        return null;
    }
}