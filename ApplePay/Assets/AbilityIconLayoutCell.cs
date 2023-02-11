using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityIconLayoutCell : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    private AbilityNodeLayout attachedLayout { get; set; }
    private AbilityCanvasRenderer layoutRenderer;
    public void LinkLayout(AbilityNodeLayout layout)
    {
        attachedLayout = layout;
    }
    public void LinkRenderer(AbilityCanvasRenderer renderer)
    {
        layoutRenderer = renderer;
    }
    public GameObject GetWorkspace() => attachedLayout.gameObject;
    public void SetActiveWorkspace(bool isActive)
    {
        attachedLayout.gameObject.SetActive(isActive);
    }
    public void DestroyCell()
    {
        if(attachedLayout != null)
            Destroy(attachedLayout);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        layoutRenderer.OnCellClicked(this);
    }
}
