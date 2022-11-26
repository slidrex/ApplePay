[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Attribute Charm", fileName = "new charm")]

public class Charm : CharmObject
{
    public EntityAttribute.TagAttribute[] TagAttributeCache;
    public System.Collections.Generic.Dictionary<int, VirtualBase> attributeFields = new System.Collections.Generic.Dictionary<int, VirtualBase>();
    public System.Collections.Generic.Dictionary<string, VirtualBase> additionalFields = new System.Collections.Generic.Dictionary<string, VirtualBase>();

    public AdditionalItemAttributes[] Attributes;
    public CharmDisplay Display;
    public CharmField[] CharmFields;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        for(int i = 0; i < Attributes.Length; i++)
        {
            attributeFields.Add(i, new VirtualBase(Attributes[i].AdditionalAttributeValue));
        }
        for(int i = 0; i < CharmFields.Length; i++)
        {
            additionalFields.Add(CharmFields[i].name, new VirtualBase(CharmFields[i].value));
        }
    }
    
    private void RemoveCharmTagAttributes(Creature entity)
    {
        for(int i = 0; i < TagAttributeCache.Length; i++)
        {
            TagAttributeCache[i].Remove();
        }
        TagAttributeCache = null;
    }
    public float GetFieldValue(string name)
    {
        for(int i = 0; i < CharmFields.Length; i++)
        {
            if(additionalFields.TryGetValue(name, out VirtualBase _base))
            {
                return _base.GetValue();
            }
        }
        throw new System.NullReferenceException("Unknown field name \""+ name +"\"");
    }

    [System.Serializable]
    public class CharmField
    {
        public string name;
        public float value;
        public Pay.Functions.Math.NumberType NumberType;
    }
    public virtual void BeginFunction(Creature entity) => SetCharmStats(entity);
    protected void SetCharmStats(Creature entity)
    {
        TagAttributeCache = new EntityAttribute.TagAttribute[attributeFields.Count];
        for(int i = 0; i < attributeFields.Count; i++)
        {
            EntityAttribute attribute = entity.FindAttribute(Attributes[i].AttributeName);
            
            int copy = i;

            if(Attributes[i].Type == EntityAttribute.AttributeType.Multiplier) TagAttributeCache[i] = attribute.AddPercent(new VirtualBase.VirtualFloatRef(() => attributeFields[copy].GetValue()), "charmStats");
            else TagAttributeCache[i] = attribute.AddAttributeValue(new VirtualBase.VirtualFloatRef(() => attributeFields[copy].GetValue()), "charmStats");
        }
    }
    public virtual void UpdateFunction(Creature entity) { }
    public virtual void EndFunction(Creature entity)
    {
        RemoveCharmTagAttributes(entity);
    }
}
[System.Serializable]
public class AdditionalItemAttributes
{
    public string AttributeName;
    public float AdditionalAttributeValue;
    public EntityAttribute.AttributeType Type;
    public Pay.Functions.Math.NumberType DisplayType;
}
public enum EffectType
{
    Unassigned,
    Positive,
    Negative,
    Neutral
}