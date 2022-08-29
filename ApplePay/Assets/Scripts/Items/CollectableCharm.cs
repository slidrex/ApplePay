public class CollectableCharm : CollectableItem
{
    public CharmItem CharmItem;
    protected override Item CollectableObject { get => CharmItem; }
}
