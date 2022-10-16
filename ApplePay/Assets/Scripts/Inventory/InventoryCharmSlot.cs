using UnityEngine;

public class InventoryCharmSlot : InventoryDisplaySlot
{
    [SerializeField] private GameObject SwitchIcon;
    public void OnCharmSwitched()
    {
        CharmRepositoryRenderer renderer = (CharmRepositoryRenderer)attachedRenderer;
        renderer.OnCharmSwitched(this);
    }
    public void RenderItem(CharmDisplay display, bool switchable)
    {
        RenderItem(display);
        SwitchIcon.SetActive(false);
        if(switchable) SwitchIcon.SetActive(true);
        switchable = false;
    }
}
