[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Rubick", fileName = "new charm")]
public class Rubick : Charm
{
    private const string tag = "rubick_attribute-amplification";
    public override void UpdateFunction(Creature entity)
    {
        base.UpdateFunction(entity);
        CharmRepository _repository = (CharmRepository)entity.InventorySystem.GetRepository("charm");
        
        CharmObject[] charmItems = _repository.Items;
        for(int i = 0; i < charmItems.Length; i++)
        {
            CharmObject charm = charmItems[i];
            if(charm != null && charm.GetActiveCharm() != this)
            {
                for(int j = 0; j < charm.GetActiveCharm().attributeFields.Count; j++)
                {
                    VirtualBase virtualBase = charm.GetActiveCharm().attributeFields[j];
                        
                    if(!virtualBase.ContainsModifiedTag(tag))
                    {
                        virtualBase.AddPercent(0.25f, tag);
                        entity.FindAttribute(charm.GetActiveCharm().Attributes[j].AttributeName).ApplyResult();
                    }
                }
                
                foreach(VirtualBase virtualBase in charm.GetActiveCharm().additionalFields.Values)
                {
                    if(!virtualBase.ContainsModifiedTag(tag)) virtualBase.AddPercent(0.25f, tag);
                }
            }
        }
        //Performance issue
    }
}