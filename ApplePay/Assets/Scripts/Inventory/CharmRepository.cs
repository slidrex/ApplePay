public class CharmRepository : InventoryRepository
{
    public override System.Type RepositoryType { get; } = typeof(CharmItem);
    public System.Collections.Generic.List<CharmItem> InventoryItems = new System.Collections.Generic.List<CharmItem>();
    protected override void Update()
    {
        foreach(CharmItem charmItem in InventoryItems) charmItem.Item.UpdateFunction();
    }
    public override void OnItemAdded(Item added)
    {
        CharmItem charmItem = (CharmItem)added;
        charmItem.Item.BeginFunction(AttachedSystem.InventoryOwner);
        InventoryItems.Add((CharmItem)added);
    }
    public override void OnItemRemoved(Item removedItem)
    {
        CharmItem charmItem = (CharmItem)removedItem;
        InventoryItems.Remove(charmItem);
        charmItem.Item.EndFunction(AttachedSystem.InventoryOwner);
    }

}