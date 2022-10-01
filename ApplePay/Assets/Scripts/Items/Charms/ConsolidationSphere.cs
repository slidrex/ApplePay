using PayWorld;
[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere")]
public class ConsolidationSphere : Charm
{
    [UnityEngine.SerializeField] private float AdditionalEffectAmplifying;
    [UnityEngine.SerializeField] private float AdditionalEffectDuration;
    private EffectController.EffectMask valueMask, durationMask;
    public override void UpdateFunction(Entity entity)
    {
        base.UpdateFunction(entity);
        foreach(PayWorld.EffectController.ActiveEffect effect in entity.ActiveEffects.Values)
        {
            if(effect.Tags.Contains("SphereAmplified") == false)
            {
                if(effect.Tags.Contains("positiveEffect"))
                {
                    durationMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.RemainTime, AttributeOperation.Multiply, AdditionalEffectDuration);
                }
                valueMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.EffectValue, AttributeOperation.Multiply, AdditionalEffectAmplifying);
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
    public override void EndFunction(Entity entity)
    {
        base.EndFunction(entity);
        if(valueMask.Equals(new EffectController.EffectMask()) == false) valueMask.Remove();
        if(durationMask.Equals(new EffectController.EffectMask()) == false) durationMask.Remove();
    }
}