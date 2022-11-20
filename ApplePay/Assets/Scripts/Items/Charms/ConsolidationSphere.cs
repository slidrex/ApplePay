using PayWorld;
[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere")]
public class ConsolidationSphere : Charm
{
    private EffectController.EffectMask valueMask, durationMask;
    public override void UpdateFunction(Creature entity, ChangeManual manual)
    {
        base.UpdateFunction(entity, manual);
        float amplify = GetFieldValue("effect-amplify", new ChangeManual()); 
        float duration = GetFieldValue("effect-duration", new ChangeManual());
        
        foreach(PayWorld.EffectController.ActiveEffect effect in entity.ActiveEffects.Values)
        {
            if(effect.Tags.Contains("SphereAmplified") == false)
            {
                if(effect.Tags.Contains("positiveEffect"))
                {
                    durationMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.RemainTime, AttributeOperation.Multiply, duration);
                }
                valueMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.EffectValue, AttributeOperation.Multiply, amplify);
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
    public override void EndFunction(Creature entity, ChangeManual manual)
    {
        base.EndFunction(entity, manual);
        if(valueMask.Equals(new EffectController.EffectMask()) == false) valueMask.Remove();
        if(durationMask.Equals(new EffectController.EffectMask()) == false) durationMask.Remove();
    }
}