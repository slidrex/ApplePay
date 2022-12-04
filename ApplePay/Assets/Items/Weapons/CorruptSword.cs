using UnityEngine;
using System.Linq;

public class CorruptSword : MeleeWeapon
{
    [Header("Sword Settings")]
    [SerializeField] private float corruptionTime;
    [SerializeField] private byte level;
    [SerializeField] private float slownessPerLevel;
    [SerializeField] private int decayHPPerLevel;
    [SerializeField] private float duration;
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
                    hitEntity.ActiveEffects[hitEntity.ActiveEffects.ElementAt(i).Key].RemainTimeSourceValue = corruptionTime;
                    return;
                }
            }
                PayWorld.EffectController.ActiveEffect effect = PayWorld.EffectController.AddEffect(hitEntity, duration, out byte id, 
                    new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.AttributePercent("movementSpeed", level * -slownessPerLevel)),
                    new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.ChangeHealth(-decayHPPerLevel, 1f))
                    );
                PayWorld.EffectController.AttachVisualAttrib(effect, "Corrpution", "Decreases movement speed by {0,-100}%. Removes {1,-1} HP every second.", PayWorld.Effect.EffectDatabase.ColorizeRoman(level, PayWorld.Effect.EffectDatabase.EffectColor.Negative), null);
                effect.Tags.Add(wrappedTag);
        }
    }
}
