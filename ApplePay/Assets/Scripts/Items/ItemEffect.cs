
[System.Serializable]
public class ItemEffect
{
    public string EffectID;
    public byte Level = 1;
    public byte BeginFunction(Entity entity)
    {
        PayWorld.EffectController.AddEffect(entity, out byte id, PayWorld.EffectController.GetEffectTemplate(EffectID, Level).StateEffects);
        return id;
    }
    public void UpdateFunction() { }
    public void EndFunction(Entity entity, ref byte effectId) 
    {
        PayWorld.EffectController.RemoveEffect(entity, ref effectId);
    }
}
