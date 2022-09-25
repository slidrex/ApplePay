using PayWorld;
[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere", fileName = "new charm")]
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
                valueMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.EffectValue, AttributeOperation.Multiply, 2f);
                if(effect.Tags.Contains("positiveEffect"))
                {
                    durationMask = effect.AddMask(EffectController.EffectMask.MaskedParameter.RemainTime, AttributeOperation.Multiply, 1.5f);
                }
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
    public override void EndFunction(Entity entity)
    {
        base.EndFunction(entity);
        valueMask.Remove();
        durationMask.Remove();
    }
}