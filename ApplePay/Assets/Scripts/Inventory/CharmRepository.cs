public class CharmRepository : InventoryRepository<CollectableCharm>
{
    public override string Id => "charm";
    public override bool AddItem(CollectableCharm item)
    {
        bool success = IsValid(); 
        if(success)
        {
            CollectableCharm charm = Instantiate(item);
            base.AddItem(charm);
            charm.gameObject.SetActive(false);
            charm.gameObject.transform.SetParent(itemInstancesContainer);
        }
        return success;
    }
    public override void OnItemAdded(CollectableCharm item)
    {
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
