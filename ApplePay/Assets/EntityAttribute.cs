using System.Linq;

public struct ReferencedAttribute
{
    public System.Func<float> Get;
    public System.Action<float> Set;
    public ReferencedAttribute(System.Func<float> getter, System.Action<float> setter)
    {
        Get = getter;
        Set = setter;
    }
}
[System.Serializable]
public class EntityAttribute
{
    internal ReferencedAttribute ReferencedAttribute;
    public System.Collections.Generic.List<TagAttribute> TaggedAttributes;
    public System.Collections.Generic.Dictionary<string, AttributeMask> TaggedAttributeMasks;
    public System.Collections.Generic.List<TagAttributeClock> ClockedTagAttributes;
    public System.Collections.Generic.List<AttributeMask> GlobalMasks;
    internal float BaseAttributevalue;
    internal float ValueMultiplier;
    public EntityAttribute(ReferencedAttribute attribute, float baseAttributeValue)
    {
        ClockedTagAttributes = new System.Collections.Generic.List<TagAttributeClock>();
        ReferencedAttribute = attribute;
        BaseAttributevalue = baseAttributeValue;
        TaggedAttributes = new System.Collections.Generic.List<TagAttribute>();
        GlobalMasks = new System.Collections.Generic.List<AttributeMask>();
        TaggedAttributeMasks = new System.Collections.Generic.Dictionary<string, AttributeMask>();
        ValueMultiplier = 1f;
    }
    public void HandleClockedAttributes()
    {
        for(int i = 0; i < ClockedTagAttributes.Count; i++)
        {
            if(ClockedTagAttributes[i].RemainTime <= 0)
            {
                ClockedTagAttributes[i].TagAttribute.Remove();
                ClockedTagAttributes.RemoveAt(i);
            }
            else
            {
                TagAttributeClock attribute = ClockedTagAttributes[i];
                attribute.RemainTime -= UnityEngine.Time.deltaTime;
                ClockedTagAttributes[i] = attribute;
            }
        }
    }
}

public enum AttributeOperation { Add, Multiply }

public enum AttributeType { Multiplier, BaseAttributeValue }

public struct AttributeMask
{
    public EntityAttribute AttachedAttribute;
    public float Value;
    public string TaggedValue;
    public AttributeOperation Operation;
    public AttributeMask(float value, AttributeOperation operation, EntityAttribute attachedAttribute, string tagged)
    {
        Value = value;
        AttachedAttribute = attachedAttribute;
        Operation = operation;
        TaggedValue = tagged;
    }
}
public class TagAttribute
{
    public EntityAttribute AttachedAttribute;
    public string[] Tags;
    public float Value;
    public AttributeType Type;
    public byte Count { get; set; }
    public TagAttribute(EntityAttribute attachedAttribute, float value, AttributeType attributeType, string[] tags)
    {
        AttachedAttribute = attachedAttribute;
        Type = attributeType;
        Tags = tags;
        if(Tags == null) Tags = new string[1];
        Value = value;
    }
}
public struct TagAttributeClock
{
    [UnityEngine.SerializeField] internal float RemainTime;
    [UnityEngine.SerializeField] internal TagAttribute TagAttribute;
    internal TagAttributeClock(TagAttribute tagAttribute, float time)
    {
        RemainTime = time;
        TagAttribute = tagAttribute;
    }
}
public struct AttributeSummary
{
    internal float BaseAttributeValue, MultiplierValue;
    public static AttributeSummary operator +(AttributeSummary first, AttributeSummary second) 
    {
        AttributeSummary summary = new AttributeSummary();
        summary.BaseAttributeValue = first.BaseAttributeValue + second.BaseAttributeValue;
        summary.MultiplierValue = first.MultiplierValue + second.MultiplierValue;
        return summary;
    }
}
public static class EntityAttributeExtension
{
    ///<summary> Sets the tagged attribute termination time.
    public static void SetDestroyClock(this TagAttribute attribute, float time)  => attribute.AttachedAttribute.ClockedTagAttributes.Add(new TagAttributeClock(attribute, time));
    public static void RemoveDestroyClock(this TagAttribute attribute)
    {
        EntityAttribute attrib = attribute.AttachedAttribute;
        for(int i = 0; i < attrib.ClockedTagAttributes.Count; i++)
        {
            if(attrib.ClockedTagAttributes[i].TagAttribute == attribute)
            {
                attrib.ClockedTagAttributes.RemoveAt(i);
            }
        }
    }
    ///<summary> Adds a mask that add change applies for each tagged attribute with the specified tag. </summry>
    public static AttributeMask AddTaggedMultiplierAttributeMask(this EntityAttribute attribute, float value, AttributeOperation operation, string maskedTag) 
    {
        AttributeMask mask = new AttributeMask(value, operation, attribute, maskedTag);
        attribute.TaggedAttributeMasks.Add(maskedTag, mask);
        attribute.ApplyResult();
        return mask;
    }
    public static AttributeMask AddGlobalMultiplierAttributeMask(this EntityAttribute attribute, float value, AttributeOperation operation)
    {
        AttributeMask mask = new AttributeMask(value, operation, attribute, null);
        attribute.GlobalMasks.Add(mask);
        attribute.ApplyResult();
        return mask;
    }
    public static void Remove(this AttributeMask mask)
    {
        if(mask.TaggedValue == null)
            mask.AttachedAttribute.GlobalMasks.Remove(mask);
        else mask.AttachedAttribute.TaggedAttributeMasks.Remove(mask.TaggedValue);
        mask.AttachedAttribute.ApplyResult();
    }
    public static EntityAttribute FindAttribute(this Entity entity, string name) 
    {
        EntityAttribute attrib = null;
        entity.Attributes.TryGetValue(name, out attrib);
        return attrib;
    }
    public static void Remove(this TagAttribute attribute)
    {
        if(attribute.Type == AttributeType.Multiplier) attribute.AttachedAttribute.ValueMultiplier -= attribute.Value;
        else attribute.AttachedAttribute.BaseAttributevalue -= attribute.Value;
        
        attribute.AttachedAttribute.TaggedAttributes.Remove(attribute);
        attribute.AttachedAttribute.ApplyResult();
    }
    public static void SetMultiplier(this EntityAttribute attribute, float value) 
    {
        attribute.ValueMultiplier = value;
        attribute.ApplyResult();
    }
    public static float GetAttributeValue(this EntityAttribute attribute) => attribute.ReferencedAttribute.Get();
    public static void SetAttributeValue(this EntityAttribute attribute, float baseAttributeValue)
    {
        attribute.BaseAttributevalue = baseAttributeValue;
        attribute.ApplyResult();
    }
    public static void AddAttributeValue(this EntityAttribute attribute, float value, AttributeType attributeType) 
    {
        if(attributeType == AttributeType.Multiplier) attribute.ValueMultiplier += value;
        else if(attributeType == AttributeType.BaseAttributeValue) attribute.BaseAttributevalue += value;
        attribute.ApplyResult();
    }
    public static TagAttribute AddAttributeValue(this EntityAttribute attribute, float value, AttributeType attributeType, params string[] tags)
    {
        TagAttribute attrib = new TagAttribute(attribute, value, attributeType, tags);
        
        if(attributeType == AttributeType.Multiplier) attribute.ValueMultiplier += value;
        else attribute.BaseAttributevalue += value;
        
        attribute.TaggedAttributes.Add(attrib);
        attribute.ApplyResult();
        return attrib;
    }
    ///<summary> Translates all the changes to the result value. </summary>
    private static void ApplyResult(this EntityAttribute attribute)
    {
        AttributeSummary globalMaskSummary = attribute.GetGlobalMasksSummary();
        AttributeSummary taggedAttribSummary = attribute.GetTaggedAttributeSummary();
        
        
        attribute.ReferencedAttribute.Set((attribute.BaseAttributevalue + taggedAttribSummary.BaseAttributeValue) * ((attribute.ValueMultiplier + taggedAttribSummary.MultiplierValue) * globalMaskSummary.MultiplierValue));
    }
    private static AttributeSummary GetGlobalMasksSummary(this EntityAttribute attribute)
    {
        AttributeSummary summary = new AttributeSummary();
        summary.MultiplierValue = 1f;
        foreach(AttributeMask mask in attribute.GlobalMasks)
        {
            summary.MultiplierValue = mask.Operation == AttributeOperation.Add ? summary.MultiplierValue + mask.Value : summary.MultiplierValue * mask.Value;
        }
        return summary;
    }
    private static AttributeSummary GetTaggedAttributeSummary(this EntityAttribute attribute)
    {
        AttributeSummary summary = new AttributeSummary();
        foreach(TagAttribute tagAttribute in attribute.TaggedAttributes)
        {
            if(tagAttribute.Type == AttributeType.BaseAttributeValue) summary.BaseAttributeValue += tagAttribute.Value;
            else
            {
                    foreach(string tag in tagAttribute.Tags)
                    {
                        if(tag != null && attribute.TaggedAttributeMasks.ContainsKey(tag))
                        {
                            attribute.TaggedAttributeMasks.TryGetValue(tag, out AttributeMask currentMask);
                            if(currentMask.Operation == AttributeOperation.Add) summary.MultiplierValue += currentMask.Value;
                            else summary.MultiplierValue *= currentMask.Value;
                        }
                    }
            }
        }
        return summary;
    }
    public static bool ContainsTaggedAttribute(this EntityAttribute attribute, string tag)
    {
        foreach(TagAttribute tagAttrib in attribute.TaggedAttributes)
        {
            if(tagAttrib.Tags.Contains(tag)) return true;
        }
        return false;
    }
    public static byte GetTaggedAttributesCount(this EntityAttribute attribute, string tag)
    {
        byte taggedAttributesCount = 0;
        foreach(TagAttribute tagAttribute in attribute.TaggedAttributes)
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
        ///<summary> Turns variable into a result attribute. </summary>
    public static EntityAttribute AddAttribute(this Entity entity, string name, ReferencedAttribute attribute, float baseAttributeValue)
    {
        EntityAttribute attrib = new EntityAttribute(attribute, baseAttributeValue);
        entity.Attributes.Add(name, attrib);
        return attrib;
    }
    public static TagAttribute[] GetTagAttributes(this EntityAttribute attribute, string tag) => attribute.TaggedAttributes.Where(x => x.Tags.Contains(tag)).ToArray();
}