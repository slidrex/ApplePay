using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Ability Node/Health Node")]
public class HealthNode : AbilityNode
{
    protected override void OnNodeBegin(Creature entity)
    {
        int dHealth = (int)GetNodeValue("Health");
        if(dHealth > 0)
        {
            entity.Heal(dHealth, entity);
        }
        else if(dHealth < 0) entity.Damage(dHealth, DamageType.Magical, entity);
    }
}
