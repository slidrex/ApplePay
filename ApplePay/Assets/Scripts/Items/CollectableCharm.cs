public class CollectableCharm : CollectableItem<CollectableCharm>
{
    public override CollectableCharm CollectableObject { get => this; }
    protected override string hoverableObjectHeader => charm.GetActiveCharm().Display.Description.Name;
    protected override string hoverableObjectDescription => charm.GetActiveCharm().Display.Description.Description;
    public Charm charm;
    protected override string TargetRepository => "charm";
    protected override void Awake()
    {
        base.Awake();
        charm = Instantiate(charm);
    }
}