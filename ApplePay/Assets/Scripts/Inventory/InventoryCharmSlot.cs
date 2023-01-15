using UnityEngine;

public class InventoryCharmSlot : InventoryDisplaySlot<CollectableCharm>
{
    [SerializeField] private GameObject SwitchIcon;
    private new CharmRepositoryRenderer attachedRenderer {get => (CharmRepositoryRenderer)base.attachedRenderer;}
    public void OnCharmSwitched()
    {
        attachedRenderer.OnCharmSwitched(this);
    }
    public void RenderSwitchIcon(bool active) => SwitchIcon.SetActive(active);
}