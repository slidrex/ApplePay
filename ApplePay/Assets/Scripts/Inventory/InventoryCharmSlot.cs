using UnityEngine;

public class InventoryCharmSlot : InventoryDisplaySlot<CharmDisplay>
{
    [SerializeField] private GameObject SwitchIcon;
    private new CharmRepositoryRenderer attachedRenderer {get => (CharmRepositoryRenderer)base.attachedRenderer;}
    public void OnCharmSwitched()
    {
        attachedRenderer.OnCharmSwitched(this);
    }
    public void RenderItem(CharmDisplay display, bool switchable)
    {
        RenderIcon(display.Icon);
        LinkDisplay(display);
        SwitchIcon.SetActive(false);
        if(switchable) SwitchIcon.SetActive(true);
        switchable = false;
    }
}
