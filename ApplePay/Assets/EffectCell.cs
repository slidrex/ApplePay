using UnityEngine;
using UnityEngine.EventSystems;
public class EffectCell : HoverableObject
{
    private EffectGrid effectGrid;
    private UnityEngine.UI.Image imageRenderer;
    [HideInInspector] public PayWorld.EffectController.EffectDisplay EffectDisplay;
    public UnityEngine.UI.Text IndexText;
    private void Start()
    {
        imageRenderer = GetComponent<UnityEngine.UI.Image>();
        effectGrid = transform.parent.GetComponent<EffectGrid>();
        
        IndexText.text = EffectDisplay.Index;
        
        UpdateCellImage();
    }
    protected override void Update()
    {
        base.Update();
    }
    public void UpdateCellImage() => imageRenderer.sprite = EffectDisplay.Sprite;
    public override void OnPointerEnter(PointerEventData pointerData)
    {
        base.OnPointerEnter(pointerData);
        effectGrid.OnCellEnter(this);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        effectGrid.OnCellExit(this);
    }
    public override void OnPointer()
    {
        base.OnPointer();
        effectGrid.OnCellOver(this);
    }
    private void OnDestroy()
    {
        effectGrid?.OnCellDestroy();
    }
}
