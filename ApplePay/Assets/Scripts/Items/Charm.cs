[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Stat Charm", fileName = "new charm")]
public class Charm : UnityEngine.ScriptableObject
{
    public AdditionalItemAttributes[] Attributes;
    public CharmDisplay Display;
    protected TagAttribute[] TaggedAttributes;
    public virtual void BeginFunction(Entity entity) => SetCharmStats(entity);
    protected void SetCharmStats(Entity entity)
    {
        TaggedAttributes = new TagAttribute[Attributes.Length];
        
        for(int i = 0; i < Attributes.Length; i++)
        {
            AdditionalItemAttributes current = Attributes[i];
            
            TagAttribute taggedAttrib = entity.FindAttribute(current.AttributeName).AddTaggedAttribute(current.AdditionalAttributeValue, current.Type, "charmStats");
            TaggedAttributes[i] = taggedAttrib;
        }
    }
    public virtual void UpdateFunction(Entity entity) { }
    public virtual void EndFunction(Entity entity)
    {
        foreach(TagAttribute tag in TaggedAttributes) tag.Remove();
    }
}
[System.Serializable]
public class CharmDisplay : ItemDisplay
{
    public ItemDescription Description;
    public CharmAddtionalField[] AdditionalFields;
    [System.Serializable]
    public struct CharmAddtionalField
    {  
        public UnityEngine.Color Color;
        public string Text;
    }
}
[System.Serializable]
public class AdditionalItemAttributes
{
    public string AttributeName;
    public float AdditionalAttributeValue;
    public AttributeType Type;
    [UnityEngine.Tooltip("Automatically defines effect color in charm hoverboard (or take Additional Field color if \"Unassigned\" is chosen).")]
    public EffectType EffectColor;
}
    public enum EffectType
    {
        Unassigned,
        Positive,
        Negative,
        Neutral
    }
public static class CharmStatExtension
{
    
}