[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Rubick", fileName = "new charm")]
public class Rubick : Charm
{
    private const string tag = "rubick_attribute-amplification";
    public override void UpdateFunction(Creature entity, ChangeManual manual)
    {
        base.UpdateFunction(entity, manual);
        InventoryRepository _repository = entity.InventorySystem.GetRepository("charms");
        Item[] charmItems = _repository.GetExistingItems();
        for(int i = 0; i < charmItems.Length; i++)
        {
            CharmItem charm = (CharmItem)charmItems[i];
            if(charm.GetActiveCharm() != this)
            {
                for(int j = 0; j < charm.Manuals[charm.ActiveIndex].attributeFields.Count; j++)
                {
                    VirtualBase virtualBase = charm.Manuals[charm.ActiveIndex].attributeFields[j];
                    if(!virtualBase.ContainsModifiedTag(tag))
                    {
                        virtualBase.AddPercent(0.25f, tag);
                        entity.FindAttribute(charm.GetActiveCharm().Attributes[j].AttributeName).ApplyResult();
                    }
                }
                
                foreach(VirtualBase virtualBase in charm.Manuals[charm.ActiveIndex].additionalFields.Values)
                {
                    if(!virtualBase.ContainsModifiedTag(tag)) virtualBase.AddPercent(0.25f, tag);
                }
            }
        }
        //Performance issue
    }
}