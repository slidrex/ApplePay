using UnityEngine;

public class InventoryCharmSlot : InventoryDisplaySlot<Charm>
{
    [SerializeField] private GameObject SwitchIcon;
    private new CharmRepositoryRenderer attachedRenderer {get => (CharmRepositoryRenderer)base.attachedRenderer;}
    public void OnCharmSwitched()
    {
        attachedRenderer.OnCharmSwitched(this);
    }
    public void RenderItem(Charm item, bool switchable)
    {
        RenderIcon(item.Display.Icon);
        LinkItem(item);
        SwitchIcon.SetActive(false);
        if(switchable) SwitchIcon.SetActive(true);
    }
}