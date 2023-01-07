using UnityEngine;

public abstract class AttributeMeleeWeapon : MeleeWeapon
{
    protected abstract string attributeTag {get;}
    [Header("Attriubre melee settings")]
    [SerializeField] private MeleeWeapon.AttackSide side;
    [SerializeField] private bool passImmortal;
    [SerializeField] private string attribute;
    [SerializeField] private float value;
    [SerializeField] private EntityAttribute.AttributeType type;
    [SerializeField] private float time;
    private EntityAttribute.TagAttribute tagAttribute;
    protected override void OnEntityHitEnter(Collider2D collision, Entity hitEntity)
    {
        base.OnEntityHitEnter(collision, hitEntity);
        Entity entity = side == AttackSide.Attacker ? Owner : hitEntity;
        EntityAttribute _attribute = entity?.FindAttribute(attribute);
        if(_attribute == null || (passImmortal == false && entity.Immortal)) return;
        string tagName = attributeTag + Owner;

        if(_attribute.GetTagAttributesCount(tagName) > 1) throw new System.Exception("Doubled effect");
        
        if(!_attribute.ContainsTagAttribute(tagName))
        {
            tagAttribute = type == EntityAttribute.AttributeType.Multiplier ? _attribute.AddAttributeMultiplier(value, tagName) : _attribute.AddAttributeValue(value, tagName);
            
        }
        else
        {
            tagAttribute = _attribute.GetTagAttributes(tagName)[0];
            
            if(tagAttribute.GetDestroyClocks().Count > 0) 
            {
                tagAttribute.GetDestroyClocks()[0].Remove(false);
            }
        }
        tagAttribute.SetDestroyClock(time);
    }
}
