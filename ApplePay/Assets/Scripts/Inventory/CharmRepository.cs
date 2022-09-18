public class CharmRepository : InventoryRepository
{
    public override System.Type RepositoryType { get; } = typeof(CharmItem);
    public System.Collections.Generic.List<CharmItem> InventoryItems = new System.Collections.Generic.List<CharmItem>();
    public System.Collections.Generic.Dictionary<CharmItem, byte> EffectBuffer = new System.Collections.Generic.Dictionary<CharmItem, byte>();
    protected override void Update()
    {
        foreach(CharmItem charmItem in InventoryItems) charmItem.Item.Effect?.UpdateFunction();
    }
    public override void OnItemAdded(Item added)
    {
        CharmItem charmItem = (CharmItem)added;
        InventoryItems.Add(charmItem);
        if(charmItem.Item.Effect != null)
        {
            byte effectID = charmItem.Item.Effect.BeginFunction(AttachedSystem.InventoryOwner);
            EffectBuffer.Add(charmItem, effectID);
        }
    }
    public override void OnItemRemoved(Item removedItem)
    {
        CharmItem charmItem = (CharmItem)removedItem;
        InventoryItems.Remove(charmItem);
        EffectBuffer.TryGetValue(charmItem, out byte effectID);
        charmItem.Item.Effect.EndFunction(AttachedSystem.InventoryOwner, ref effectID);
    }

}