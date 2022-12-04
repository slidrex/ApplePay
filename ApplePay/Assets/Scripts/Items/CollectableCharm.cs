public class CollectableCharm : CollectableItem<Charm>
{
    protected override Charm CollectableObject { get => charm; set => charm = value; }
    protected override string hoverableObjectHeader => charm.GetActiveCharm().Display.Description.Name;
    protected override string hoverableObjectDescription => charm.GetActiveCharm().Display.Description.Description;
    public Charm charm;
    protected override string TargetRepository => "charm";
}
