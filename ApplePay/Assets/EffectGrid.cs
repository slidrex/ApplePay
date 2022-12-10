using UnityEngine;

public class EffectGrid : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private EffectPanel effectPanel;
    [HideInInspector] public EffectPanel CurrentEffectPanel;
    private float offset = 0.5f;
    public void OnCellEnter(EffectCell cell)
    {
        PanelSet(cell);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(CurrentEffectPanel.gameObject.GetComponent<RectTransform>());
   }
    private void PanelSet(EffectCell cell)
    {
        CurrentEffectPanel = Instantiate(effectPanel, Vector3.zero, Quaternion.identity);
        CurrentEffectPanel.gameObject.transform.SetParent(canvas.transform);
        CurrentEffectPanel.gameObject.transform.localScale = Vector3.one;
        CurrentEffectPanel.SetHeader(cell.EffectDisplay.Name);
        if(cell.EffectDisplay.Additionals != null)
            for(int i = 0; i < cell.EffectDisplay.Additionals.Length; i++) CurrentEffectPanel.CreateTextField(cell.EffectDisplay.Additionals[i].TextConfiguration, cell.EffectDisplay.Additionals[i].Text, 1);
    }
    private Vector2 GetFixedOffset(RectTransform rectTransform, Vector2 direction, float offset)
    {
        Vector3[] verteces = new Vector3[4];
        rectTransform.GetWorldCorners(verteces);
        Vector2 mousePos = Pay.Functions.Generic.GetMousePos(Camera.main);
        return mousePos + Mathf.Abs((Screen.width - rectTransform.rect.width)/2) * Vector2.right * 0.01f;
    }
    public void OnCellOver(EffectCell cell)
    {
        
        
        CurrentEffectPanel.SetMain(cell.EffectDisplay.FormatDescription(cell.EffectDisplay.Description));
        
        
        
        CurrentEffectPanel.transform.position = GetFixedOffset(CurrentEffectPanel.GetComponent<RectTransform>(), Vector2.right, offset);
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