using UnityEngine;

public class EffectGrid : MonoBehaviour
{
    [SerializeField, ReadOnly] private Canvas canvas;
    [SerializeField] private EffectPanel effectPanel;
    [HideInInspector] public EffectPanel CurrentEffectPanel;
    [SerializeField] private Vector2 offset;
    public void OnCellEnter(EffectCell cell)
    {
        PanelSet(cell);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(CurrentEffectPanel.gameObject.GetComponent<RectTransform>());
   }
    private void PanelSet(EffectCell cell)
    {
        GameObject panelObj = Instantiate(effectPanel.gameObject, Vector3.zero, Quaternion.identity);
        panelObj.transform.SetParent(canvas.transform);
        panelObj.transform.localScale = Vector3.one;
        CurrentEffectPanel = panelObj.GetComponent<EffectPanel>();
        
        CurrentEffectPanel.SetHeader(cell.EffectDisplay.Name);
        CurrentEffectPanel.SetMain(cell.EffectDisplay.Description);
        for(int i = 0; i < cell.EffectDisplay.Additionals.Length; i++) CurrentEffectPanel.CreateTextField(cell.EffectDisplay.Additionals[i].TextConfiguration, cell.EffectDisplay.Additionals[i].Text, 1);
    }
    public void OnCellOver(EffectCell cell)
    {
        Vector2 nativeOffset = Vector2.right/3;
        Vector2 res = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        Vector2 aspectRatio = new Vector2(res.x/res.y, res.y/res.x);
        
        
        Vector2 additionalScale = CurrentEffectPanel.GetComponent<RectTransform>().rect.size * canvas.transform.lossyScale;
        Vector2 mousePos = Pay.Functions.Generic.GetMousePos(Camera.main) + (offset + nativeOffset) * additionalScale * aspectRatio;
        if(CurrentEffectPanel != null) CurrentEffectPanel.transform.position = mousePos;
    }
    public void OnCellExit(EffectCell cell)
    {
        if(CurrentEffectPanel != null) Destroy(CurrentEffectPanel.gameObject);
    }
    public void OnCellDestroy()
    {
        if(CurrentEffectPanel != null) Destroy(CurrentEffectPanel.gameObject);
    }
}