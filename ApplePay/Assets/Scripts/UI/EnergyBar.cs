public class EnergyBar : EntityIndicatorBar
{
    protected IEnergyConsumer Consumer;
    protected override float BarCurrentValue => Consumer.Consumer.CurrentEnergy;
    protected override float BarMaxValue => Consumer.Consumer.MaxEnergy;
    public override void IndicatorSetup(Entity entity)
    {
        base.IndicatorSetup(entity);
        Consumer = entity as IEnergyConsumer;
    }
}
