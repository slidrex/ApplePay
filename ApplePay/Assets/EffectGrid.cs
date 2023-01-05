using UnityEngine;

public class EffectGrid : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private EffectPanel effectPanel;
    [HideInInspector] public EffectPanel CurrentEffectPanel;
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
    public void OnCellOver(EffectCell cell)
    {
        
        
        CurrentEffectPanel.SetMain(cell.EffectDisplay.FormatDescription(cell.EffectDisplay.Description));
        
        float aspectRatio = (float)Screen.width/Screen.height;
        CurrentEffectPanel.transform.position = cell.transform.position;
        
        CurrentEffectPanel.transform.position = cell.transform.position - Vector3.up * 0.45f * aspectRatio;
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