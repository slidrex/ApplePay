[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Attribute Charm", fileName = "new charm")]

public class Charm : CharmObject
{
    public AdditionalItemAttributes[] Attributes;
    public CharmDisplay Display;
    public CharmField[] CharmFields;
    public virtual void ReloadCharmManual(Creature entity, ChangeManual manual)
    {
        EndFunction(entity, manual);
        BeginFunction(entity, manual);
    }
    private void RemoveCharmTagAttributes(ChangeManual manual, Creature entity)
    {
        for(int i = 0; i < manual.TagAttributeCache.Count; i++)
        {
            manual.TagAttributeCache[i].Remove();
        }
    }
    public float GetFieldValue(string name, ChangeManual manual)
    {
        for(int i = 0; i < CharmFields.Length; i++)
        {
            if(manual.additionalFields.TryGetValue(name, out VirtualBase _base))
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
    public virtual void BeginFunction(Creature entity, ChangeManual manual) => SetCharmStats(entity, manual);
    protected void SetCharmStats(Creature entity, ChangeManual manual)
    {
        for(int i = 0; i < manual.attributeFields.Count; i++)
        {
            EntityAttribute attribute = entity.FindAttribute(Attributes[i].AttributeName);
            
            int copy = i;

            if(Attributes[i].Type == EntityAttribute.AttributeType.Multiplier) manual.TagAttributeCache.Add(attribute.AddPercent(new VirtualBase.VirtualFloatRef(() => manual.attributeFields[copy].GetValue()), "charmStats"));
            else manual.TagAttributeCache.Add(attribute.AddAttributeValue(new VirtualBase.VirtualFloatRef(() => manual.attributeFields[copy].GetValue()), "charmStats"));
        }
    }
    public virtual void UpdateFunction(Creature entity, ChangeManual manual) { }
    public virtual void EndFunction(Creature entity, ChangeManual manual)
    {
        RemoveCharmTagAttributes(manual, entity);
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