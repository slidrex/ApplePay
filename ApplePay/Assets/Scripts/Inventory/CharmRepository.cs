public class CharmRepository : InventoryRepository<CollectableCharm>
{
    public override string Id => "charm";
    public override void OnItemAdded(CollectableCharm item, int index)
    {
        item.gameObject.SetActive(false);
        item.transform.SetParent(itemInstancesContainer);
        item.Charm.GetActiveCharm().BeginFunction(AttachedSystem.SystemOwner);
    }
    private void Update()
    {
        foreach(CollectableCharm Charm in Items)
        {
            if(Charm != null)
                Charm.Charm.GetActiveCharm().UpdateFunction(AttachedSystem.SystemOwner);
        }
    }
    public override void OnItemRemoved(CollectableCharm item)
    {
        base.OnItemRemoved(item);
        item.Charm.GetActiveCharm().EndFunction(AttachedSystem.SystemOwner); 
    }
}
