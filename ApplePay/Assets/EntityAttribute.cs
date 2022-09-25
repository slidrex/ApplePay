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

public class EntityAttribute
{
    internal ReferencedAttribute ReferencedAttribute;
    public System.Collections.Generic.List<TagAttribute> TaggedAttributes;
    public System.Collections.Generic.Dictionary<string, AttributeMask> Masks = new System.Collections.Generic.Dictionary<string, AttributeMask>();
    internal float BaseAttributevalue;
    internal float ValueMultiplier = 1f;
    public EntityAttribute(ReferencedAttribute attribute, float baseAttributeValue)
    {
        ReferencedAttribute = attribute;
        BaseAttributevalue = baseAttributeValue;
        TaggedAttributes = new System.Collections.Generic.List<TagAttribute>();
    }
}

public enum AttributeOperation { Add, Multiply }

public enum AttributeType { Multiplier, BaseAttributeValue }

public class AttributeMask
{
    public EntityAttribute AttachedAttribute;
    public string MaskTag;
    public float Value;
    public AttributeOperation Operation;
    public AttributeType ChangeType;
    public AttributeMask(float value, AttributeOperation operation, AttributeType changeType, EntityAttribute attachedAttribute, string maskedTag)
    {
        Value = value;
        MaskTag = maskedTag;
        AttachedAttribute = attachedAttribute;
        Operation = operation;
        ChangeType = changeType;
    }
}
public class TagAttribute
{
    public EntityAttribute AttachedAttribute;
    public string[] Tags;
    public float Value;
    public AttributeType Type;
    public byte Count;
}
public struct TagAttributeSummary
{
    internal float BaseAttributeValue, MultiplierValue;
    public static TagAttributeSummary operator +(TagAttributeSummary first, TagAttributeSummary second) 
    {
        TagAttributeSummary summary = new TagAttributeSummary();
        summary.BaseAttributeValue = first.BaseAttributeValue + second.BaseAttributeValue;
        summary.MultiplierValue = first.MultiplierValue + second.MultiplierValue;
        return summary;
    }
}
public static class EntityAttributeExtension
{
    ///<summary> Adds a mask that add change applies for each tagged attribute with specified tag. </summry>
    public static AttributeMask AddAttributeMask(this EntityAttribute attribute, string maskedTag, float value, AttributeOperation operation, AttributeType changeType) 
    {
        AttributeMask mask = new AttributeMask(value, operation, changeType, attribute, maskedTag);
        attribute.Masks.Add(maskedTag, mask);
        attribute.ApplyResult();
        return mask;
    }
    public static void Remove(this AttributeMask mask)
    {
        mask.AttachedAttribute.Masks.Remove(mask.MaskTag);
        mask.AttachedAttribute.ApplyResult();
    }
    public static bool ContainsAttributeMask(this EntityAttribute attribute, string tag) => attribute.Masks.ContainsKey(tag);
    private static AttributeMask GetAttributeMask(this EntityAttribute attribute, string tag) 
    {
        attribute.Masks.TryGetValue(tag, out AttributeMask mask);
        return mask;
    }
    ///<summary> Turns variable into a result attribute. </summary>
    public static EntityAttribute AddAttribute(this Entity entity, string name, ReferencedAttribute attribute, float baseAttributeValue)
    {
        EntityAttribute attrib = new EntityAttribute(attribute, baseAttributeValue);
        entity.Attributes.Add(name, attrib);
        return attrib;
    }
    public static EntityAttribute FindAttribute(this Entity entity, string name) 
    {
        EntityAttribute attrib = null;
        entity.Attributes.TryGetValue(name, out attrib);
        if(attrib == null) UnityEngine.Debug.LogWarning("Attribute \"" + name + "\" hasn't been found.");
        return attrib;
    }
    public static void Remove(this TagAttribute attribute)
    {
        if(attribute.Type == AttributeType.Multiplier) attribute.AttachedAttribute.ValueMultiplier -= attribute.Value;
        else attribute.AttachedAttribute.BaseAttributevalue -= attribute.Value;
        
        attribute.AttachedAttribute.TaggedAttributes.Remove(attribute);
        attribute.AttachedAttribute.ApplyResult();
    }
    public static void AddMultiplier(this EntityAttribute attribute, float value) => attribute.SetMultiplier(attribute.ValueMultiplier + value);
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
    private static void ApplyResultMasks(EntityAttribute attribute, out TagAttributeSummary additionalMaskedAttributes)
    {
        TagAttributeSummary resultSummary = new TagAttributeSummary();
        foreach(TagAttribute current in attribute.TaggedAttributes)
        {
            ApplyMask(attribute, current, out TagAttributeSummary summary);
            resultSummary += summary;
        }
        additionalMaskedAttributes = resultSummary;
    }
    private static void ApplyMask(EntityAttribute attribute, TagAttribute taggedAttribute, out TagAttributeSummary additionalMaskedAttributes)
    {
        TagAttributeSummary summary = new TagAttributeSummary();
        foreach(string tag in taggedAttribute.Tags)
        {
            if(attribute.Masks.ContainsKey(tag))
            {
                attribute.Masks.TryGetValue(tag, out AttributeMask mask);
                if(mask.ChangeType == taggedAttribute.Type)
                {
                    if(mask.ChangeType == AttributeType.BaseAttributeValue) summary.BaseAttributeValue += mask.Value;
                    else summary.MultiplierValue += mask.Value;
                }
            }
        }
        additionalMaskedAttributes = summary;
    }
    ///<summary> Translates all the changes to result value. </summary>
    private static void ApplyResult(this EntityAttribute attribute)
    {
        ApplyResultMasks(attribute, out TagAttributeSummary summary);
        
        attribute.ReferencedAttribute.Set((attribute.BaseAttributevalue + summary.BaseAttributeValue) * (attribute.ValueMultiplier + summary.MultiplierValue));
    }
    public static TagAttribute AddTaggedAttribute(this EntityAttribute attribute, float value, AttributeType changeValue, params string[] tags)
    {
        TagAttribute attrib = new TagAttribute();
        attrib.Type = changeValue;
        attrib.AttachedAttribute = attribute;
        attrib.Tags = tags;
        attrib.Value = value;
        
        if(changeValue == AttributeType.Multiplier) attribute.ValueMultiplier += value;
        else attribute.BaseAttributevalue += value;
        
        attribute.TaggedAttributes.Add(attrib);
        attribute.ApplyResult();
        return attrib;
    }
    public static TagAttribute SetTaggedAttribute(this EntityAttribute attribute, float value, AttributeType changeValue, params string[] tags)
    {
        TagAttribute attrib = new TagAttribute();
        attrib.Type = changeValue;
        attrib.AttachedAttribute = attribute;
        attrib.Tags = tags;
        
        if(changeValue == AttributeType.Multiplier)
        {
            attrib.Value = value - attribute.ValueMultiplier;
            SetMultiplier(attribute, value);
        }
        else 
        {
            attrib.Value = value - attribute.GetAttributeValue();
            SetAttributeValue(attribute, value);
        }
        attribute.TaggedAttributes.Add(attrib);
        return attrib;
    }
    public static bool ContainsTaggedAttribute(this EntityAttribute attribute, string tag)
    {
        foreach(TagAttribute tagAttrib in attribute.TaggedAttributes)
        {
            if(tagAttrib.Tags.Contains(tag)) return true;
        }
        return false;
    }
    public static TagAttribute[] GetTaggedAttributes(this EntityAttribute attribute, string tag)
    {
        System.Collections.Generic.List<TagAttribute> attributes = new System.Collections.Generic.List<TagAttribute>();
        foreach(TagAttribute tagAttribute in attribute.TaggedAttributes)
        {
            foreach(string _tag in tagAttribute.Tags)
            {
                if(_tag == tag) 
                {
                    attributes.Add(tagAttribute);
                    break;
                }
            }
        }
        return attributes.ToArray();
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
    public static TagAttributeSummary GetTagAttributeSummary(this EntityAttribute attribute, string tag)
    {
        TagAttributeSummary summary  = new TagAttributeSummary();
        TagAttribute[] attributes = GetTagAttributes(attribute, tag);
        
        for(int i = 0; i <  attributes.Length; i++)
        {
            TagAttribute current = attributes[i];
            if(current.Type == AttributeType.BaseAttributeValue) summary.BaseAttributeValue += current.Value;
            else summary.MultiplierValue += current.Value;
            ApplyMask(attribute, current, out TagAttributeSummary curSummary);
            summary += curSummary;
            attributes[i] = current;
        }
        return summary;
    }
    public static TagAttribute[] GetTagAttributes(this EntityAttribute attribute, string tag) => attribute.TaggedAttributes.Where(x => x.Tags.Contains(tag)).ToArray();
}