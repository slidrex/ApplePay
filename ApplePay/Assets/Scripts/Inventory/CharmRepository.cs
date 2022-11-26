public class CharmRepository : InventoryRepository<Charm>
{
    public override string Id => "charm";
    public override bool AddItem(Charm item)
    {
        Charm charmInstance = Instantiate(item);
        bool success = base.AddItem(charmInstance);
        if(success) charmInstance.BeginFunction(AttachedSystem.SystemOwner);
        return success;
    }
    private void Update()
    {
        foreach(CharmObject charm in Items)
        {
            if(charm != null)
                charm.GetActiveCharm().UpdateFunction(AttachedSystem.SystemOwner);
        }
    }
    public override bool RemoveItem(Charm item)
    {
        bool success = base.RemoveItem(item);
        if(success) item.EndFunction(AttachedSystem.SystemOwner); 
        return success;
    }
}
