using UnityEngine;

public class EffectGrid : MonoBehaviour
{
    [SerializeField, ReadOnly] private Canvas canvas;
    [SerializeField] private EffectPanel effectPanel;
    [HideInInspector] public EffectPanel CurrentEffectPanel;
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
        Vector2 nativeOffset = Vector2.right * 5f;
        Vector3[] verteces = new Vector3[4];
        CurrentEffectPanel.GetComponent<RectTransform>().GetWorldCorners(verteces);
        Vector2 scale = Vector2.one;
        
        Vector2 mousePos = Pay.Functions.Generic.GetMousePos(Camera.main) + nativeOffset /  Pay.Functions.Generic.GetAspectRatio(Camera.main);
        
        
        
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