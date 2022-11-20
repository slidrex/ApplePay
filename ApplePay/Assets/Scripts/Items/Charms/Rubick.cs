[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Rubick", fileName = "new charm")]
public class Rubick : Charm
{
    public override void UpdateFunction(Creature entity, ChangeManual manual)
    {
        base.UpdateFunction(entity, manual);
        InventoryRepository _repository = entity.InventorySystem.GetRepository("charms");
        Item[] charmItems = _repository.GetExistingItems();
        for(int i = 0; i < charmItems.Length; i++)
        {
            CharmItem charm = (CharmItem)charmItems[i];
            if(charm.Manual.ContainsMultiplier("attributes") == false)
            {
                float attribAmplif = GetFieldValue("attribute-amplification", manual);
                
                if(charm.GetActiveCharm() != this)
                {
                    ChangeManual currentManual = charm.Manual;
                        currentManual.AddMultiplier("attributes", 1 + attribAmplif);
                        charm.GetActiveCharm().ReloadCharmManual(entity, currentManual);
                }
            }

            }
        }
    }