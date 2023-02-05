using UnityEngine;

public class AbilityRepository : InventoryRepository<CollectableAbility>
{
    public override string Id => "ability";
    public override void OnItemAdded(CollectableAbility item, int index)
    {
        item.gameObject.SetActive(false);
        item.transform.SetParent(itemInstancesContainer);
    }
}
