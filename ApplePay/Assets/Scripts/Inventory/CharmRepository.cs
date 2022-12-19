public class CharmRepository : InventoryRepository<CollectableCharm>
{
    public override string Id => "charm";
    public override void OnItemAdded(CollectableCharm item, int index)
    {
        item.gameObject.SetActive(false);
        item.gameObject.transform.SetParent(itemInstancesContainer);
        item.charm.BeginFunction(AttachedSystem.SystemOwner);
    }
    private void Update()
    {
        foreach(CollectableCharm charm in Items)
        {
            if(charm != null)
                charm.charm.GetActiveCharm().UpdateFunction(AttachedSystem.SystemOwner);
        }
    }
    public override void OnItemRemoved(CollectableCharm item)
    {
        base.OnItemRemoved(item);
        item.charm.EndFunction(AttachedSystem.SystemOwner); 
    }
}
