[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Other/Consolidation sphere", fileName = "new charm")]
public class ConsolidationSphere : Charm
{
    [UnityEngine.SerializeField] private float AdditionalEffectAmplifying;
    [UnityEngine.SerializeField] private float AdditionalEffectDuration;
    public override void UpdateFunction(Entity entity)
    {
        base.UpdateFunction(entity);
        foreach(PayWorld.EffectController.ActiveEffect effect in entity.ActiveEffects.Values)
        {
            if(effect.Tags.Contains("SphereAmplified")) continue;
            foreach(PayWorld.Effect.EffectProperty property in effect.EffectProperties)
            {
                property.StateEffect.Value.Value *= AdditionalEffectAmplifying;
            }
            if(effect.Tags.Contains("positiveEffect"))
            {
                effect.RemainTime *= AdditionalEffectDuration;
                effect.Tags.Add("SphereAmplified");
            }
        }
    }
}