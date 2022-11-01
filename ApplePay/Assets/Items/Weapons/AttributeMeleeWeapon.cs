using UnityEngine;

public abstract class AttributeMeleeWeapon : MeleeWeapon
{
    protected abstract string attributeTag {get;}
    [SerializeField] private MeleeWeapon.AttackSide side;
    [SerializeField] private bool passImmortal;
    [SerializeField] private string attribute;
    [SerializeField] private float value;
    [SerializeField] private AttributeType type;
    [SerializeField] private float time;
    [SerializeField] private TagAttribute tagAttribute;
    protected override void OnEntityHitEnter(Collider2D collision, Entity hitEntity)
    {
        base.OnEntityHitEnter(collision, hitEntity);
        Entity entity = side == AttackSide.Attacker ? Owner : hitEntity;
        EntityAttribute _attribute = entity.FindAttribute(attribute);
        if(_attribute == null || (passImmortal == false && entity.Immortal)) return;
        string tagName = attributeTag + Owner;

        if(_attribute.GetTaggedAttributesCount(tagName) > 1) throw new System.Exception("Doubled effect");
        
        if(_attribute.ContainsTaggedAttribute(tagName) == false)
        {
            tagAttribute = _attribute.AddAttributeValue(value, type, tagName);
        }
        else
        {
            tagAttribute = _attribute.GetTagAttributes(tagName)[0];
            tagAttribute.RemoveDestroyClock();
        }
        tagAttribute.SetDestroyClock(time);
    }
}
