using System.Linq;

[System.Serializable]
public class EntityAttribute
{
    public IAttributable AttributeSource;
    public System.Collections.Generic.List<TagAttribute> TaggedAttributes;
    public string Name;
    [UnityEngine.SerializeField] internal float ValueMultiplier;
    public EntityAttribute(IAttributable source, string name)
    {
        AttributeSource = source;
        Name = name;
        ValueMultiplier = 1;
        TaggedAttributes = new System.Collections.Generic.List<TagAttribute>();
    }
}
public struct TagAttribute
{
    public string[] Tags;
    public float Value;
    public AttributeChangeType Type;
   public enum AttributeChangeType
    {
        Multiplier,
        Unit
    }   
}
public struct TagAttributeSummary
{
    internal float AttributeValue;
    internal float MultiplierValue;
}
public static class EntityAttributeExtension
{
    public static float GetMultiplier(this EntityAttribute attribute) => attribute.ValueMultiplier;
    public static EntityAttribute AddMultiplier(this EntityAttribute attribute, float value, params string[] tags) => SetMultiplier(attribute, GetMultiplier(attribute) + value);
    public static EntityAttribute SetMultiplier(this EntityAttribute attribute, float value, params string[] tags) 
    {
        float oldMultiplier = attribute.ValueMultiplier;
        if(oldMultiplier != 0) attribute.AttributeSource.AttributeValue /= oldMultiplier;
        attribute.AttributeSource.AttributeValue *= value;
        attribute.ValueMultiplier = value;
        
        for(int i = 0; i < attribute.TaggedAttributes.Count; i++)
        {
            if(attribute.TaggedAttributes[i].Tags == tags && attribute.TaggedAttributes[i].Type == TagAttribute.AttributeChangeType.Multiplier)
            {
                TagAttribute tag = attribute.TaggedAttributes[i];
                tag.Value += (value - oldMultiplier);
                attribute.TaggedAttributes[i] = tag;
                return attribute;
            }
        }
        return attribute;
    }
    public static float GetAttributeValue(this EntityAttribute attribute) => attribute.AttributeSource.AttributeValue;
    public static void SetAttributeValue(this EntityAttribute attribute, float value, params string[] tags)
    {
        attribute.AttributeSource.AttributeValue = value * attribute.ValueMultiplier;
        for(int i = 0; i < attribute.TaggedAttributes.Count; i++)
        {
            if(attribute.TaggedAttributes[i].Tags == tags && attribute.TaggedAttributes[i].Type == TagAttribute.AttributeChangeType.Unit)
            {
                TagAttribute tag = attribute.TaggedAttributes[i];
                tag.Value += (value - attribute.GetAttributeValue()) * attribute.ValueMultiplier;
                attribute.TaggedAttributes[i] = tag;
                return;
            }
        }
    }
    public static void AddAttributeValue(this EntityAttribute attribute, float value, params string[] tags) => attribute.SetAttributeValue((attribute.GetAttributeValue()/attribute.ValueMultiplier) + value);
    public static TagAttributeSummary GetTagAttributeSummary(this EntityAttribute attribute, string tag)
    {
        TagAttributeSummary summary  = new TagAttributeSummary();
        TagAttribute[] attributes = GetTagAttributes(attribute, tag);
        foreach(TagAttribute _attribute in attributes)
        {
            if(_attribute.Type == TagAttribute.AttributeChangeType.Unit) summary.AttributeValue += _attribute.Value;
            else summary.MultiplierValue += _attribute.Value;
        }
        summary.AttributeValue *= summary.MultiplierValue;
        return summary;
    }
    public static TagAttribute[] GetTagAttributes(this EntityAttribute attribute, string tag) => attribute.TaggedAttributes.Where(x => x.Tags.Contains(tag)).ToArray();
}