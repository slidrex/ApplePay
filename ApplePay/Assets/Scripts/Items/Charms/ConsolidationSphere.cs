[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere")]

public class ConsolidationSphere : Charm
{
    private PayWorld.EffectController.ActiveEffect.Modifier valueModifier, durationModifier;
    public override void UpdateFunction(Creature entity)
    {
        base.UpdateFunction(entity);
        float amplify = GetFieldValue("effect-amplify"); 
        float duration = GetFieldValue("effect-duration");
        
        foreach(PayWorld.EffectController.ActiveEffect effect in entity.ActiveEffects.Values)
        {
            if(effect.Tags.Contains("SphereAmplified") == false)
            {
                if(effect.Tags.Contains("positiveEffect"))
                {
                   durationModifier = effect.AddRemainTimeModifier(new VirtualBase.VirtualFloatRef(() => duration));
                }
                valueModifier = effect.AddValueModifier(new VirtualBase.VirtualFloatRef(() => amplify));
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
    public override void EndFunction(Creature entity)
    {
        base.EndFunction(entity);
        if(valueModifier.Equals(default(PayWorld.EffectController.ActiveEffect.Modifier)) == false) valueModifier.Remove();
        if(durationModifier.Equals(default(PayWorld.EffectController.ActiveEffect.Modifier)) == false) durationModifier.Remove();
    }
}