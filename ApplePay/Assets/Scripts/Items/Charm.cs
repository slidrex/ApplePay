[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Attribute Charm", fileName = "new charm")]

public class Charm : CharmObject
{
    public AdditionalItemAttributes[] Attributes;
    public CharmDisplay Display;
    public CharmAttribute[] CharmFields;
    public virtual void ReloadCharmManual(Creature entity, ChangeManual manual)
    {
        EndFunction(entity, manual);
        BeginFunction(entity, manual);
    }
    private void RemoveCharmTagAttributes(ChangeManual manual)
    {
        for(int i = 0; i < manual.TagAttributes.Length; i++)
        {
            manual.TagAttributes[i].Remove();
            manual.TagAttributes[i] = null;
        }
    }
    public float GetFieldValue(string name, ChangeManual manual)
    {
        for(int i = 0; i < CharmFields.Length; i++)
        {
            if(CharmFields[i].name == name)
            {
                float value = CharmFields[i].value * CharmFields[i].multiplier;
                
                if(manual.TryGetMultiplier(name, out float v))
                {
                    value *= v;
                }
                return value;
            }
        }
        throw new System.NullReferenceException("Unknown field name \""+ name +"\"");
    }

    [System.Serializable]
    public class CharmAttribute
    {
        public string name;
        public float value;
        public float multiplier {get; set;} = 1f;
        public Pay.Functions.Math.NumberType NumberType;
    }
    public virtual void BeginFunction(Creature entity, ChangeManual manual) => SetCharmStats(entity, manual);
    protected void SetCharmStats(Creature entity, ChangeManual manual)
    {
        manual.TagAttributes = new TagAttribute[Attributes.Length];
        for(int i = 0; i < Attributes.Length; i++)
        {
            AdditionalItemAttributes attribute = Attributes[i];
            float additional = 1f;
            if(manual.TryGetMultiplier("attributes", out float add))
            {
                additional = add;
            }
            TagAttribute taggedAttrib = entity.FindAttribute(attribute.AttributeName).AddAttributeValue(attribute.AdditionalAttributeValue * additional, attribute.Type, "charmStats");
            manual.TagAttributes[i] = taggedAttrib;
        }

        
    }
    public virtual void UpdateFunction(Creature entity, ChangeManual manual) { }
    public virtual void EndFunction(Creature entity, ChangeManual manual)
    {
        RemoveCharmTagAttributes(manual);
    }
}
[System.Serializable]
public class AdditionalItemAttributes
{
    public string AttributeName;
    public float AdditionalAttributeValue;
    public AttributeType Type;
    public Pay.Functions.Math.NumberType DisplayType;
}
public enum EffectType
{
    Unassigned,
    Positive,
    Negative,
    Neutral
}