using PayWorld;
[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere")]
public class ConsolidationSphere : Charm
{
    private EffectController.EffectMask valueMask, durationMask;
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
                    durationMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.RemainTime, EntityAttribute.AttributeOperation.Multiply, duration);
                }//To do:
                 //Ref sphere virtual database amount to effect virtual databsase remain time and effect value.
                valueMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.EffectValue, EntityAttribute.AttributeOperation.Multiply, amplify);
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
    public override void EndFunction(Creature entity)
    {
        base.EndFunction(entity);
        if(valueMask.Equals(new EffectController.EffectMask()) == false) valueMask.Remove();
        if(durationMask.Equals(new EffectController.EffectMask()) == false) durationMask.Remove();
    }
}