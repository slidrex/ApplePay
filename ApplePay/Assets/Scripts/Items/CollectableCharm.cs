public class CollectableCharm : CollectableItem
{
    public CharmItem CharmItem;
    public override string TargetRepository { get => "charms"; }
    protected override Item CollectableObject { get => CharmItem; }
}
