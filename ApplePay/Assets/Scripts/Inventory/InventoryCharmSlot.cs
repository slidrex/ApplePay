using UnityEngine;

public class InventoryCharmSlot : InventoryDisplaySlot<CollectableCharm>
{
    [SerializeField] private GameObject SwitchIcon;
    private new CharmRepositoryRenderer attachedRenderer {get => (CharmRepositoryRenderer)base.attachedRenderer;}
    public void OnCharmSwitched()
    {
        attachedRenderer.OnCharmSwitched(this);
    }
    public void RenderItem(CollectableCharm item, bool switchable)
    {
        RenderIcon(item.charm.Display.Icon);
        LinkItem(item);
        SwitchIcon.SetActive(false);
        if(switchable) SwitchIcon.SetActive(true);
    }
}