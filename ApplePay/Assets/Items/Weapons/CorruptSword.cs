using UnityEngine;
using System.Linq;

public class CorruptSword : MeleeWeapon
{
    [Header("Sword Settings")]
    [SerializeField] private float corruptionTime;
    [SerializeField] private byte level;
    protected override void OnEntityHitEnter(Collider2D collision, Entity hitEntity)
    {
        base.OnEntityHitEnter(collision, hitEntity);
        if(!hitEntity.Immortal)
        {
            string wrappedTag = "corrupt_sword_"+Owner+"_effect";
            for(int i = 0; i < hitEntity.ActiveEffects.Count; i++)
            {
                if(hitEntity.ActiveEffects.ElementAt(i).Value.Tags.Contains(wrappedTag))
                {
                    hitEntity.ActiveEffects[hitEntity.ActiveEffects.ElementAt(i).Key].RemainTime = corruptionTime;
                    return;
                }
            }
            PayWorld.EffectController.ActiveEffect effect = PayWorld.EffectController.AddEffect(hitEntity, "decay", level, corruptionTime);
            effect.Tags.Add(wrappedTag);
        }
    }
}
