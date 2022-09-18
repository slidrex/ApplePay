[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Stat Charm", fileName = "new charm")]
public class Charm : UnityEngine.ScriptableObject
{
    public AdditionalItemAttributes[] Attributes;
    public CharmDisplay Display;
    public virtual void BeginFunction(Entity entity) => SetCharmStats(entity, true);
    protected void SetCharmStats(Entity entity, bool positive)
    {
        foreach(AdditionalItemAttributes attribute in Attributes)
        {
            float sign = positive == true ? 1 : -1;
            EntityAttribute attrib = entity.FindAttribute(attribute.AttributeName);
            if(attribute.Type == TagAttribute.AttributeChangeType.Unit)
            {
                entity.FindAttribute(attribute.AttributeName).AddAttributeValue(attribute.AdditionalAttributeValue * sign, "charmStats");
            }
            else entity.FindAttribute(attribute.AttributeName).AddMultiplier(attribute.AdditionalAttributeValue * sign, "charmStats");
            
        }

    }
    public void UpdateFunction() { }
    public void EndFunction(Entity entity) => SetCharmStats(entity, false);
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
    public TagAttribute.AttributeChangeType Type;
    [UnityEngine.Tooltip("Automatically defines effect color in charm hoverboard (or take Additional Field color if \"Unassigned\" is chosen).")]
    public EffectStatus EffectColor;
    public enum EffectStatus
    {
        Unassigned,
        Positive,
        Negative,
        Neutral
    }
}
public static class CharmStatExtension
{
    
}