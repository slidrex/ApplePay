using UnityEngine;

public class ContentPanel : MonoBehaviour
{
    public UnityEngine.UI.Text TextField;
    public void CreateTextField(Pay.UI.TextConfiguration textConfiguration, string text, int verticalOffset)
    {
        UnityEngine.UI.Text obj = Instantiate(TextField, transform.position, Quaternion.identity);
        obj.gameObject.transform.SetParent(transform);
        obj.gameObject.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().padding.top = verticalOffset;
        obj.lineSpacing = textConfiguration.LineSpacing;
        obj.font = textConfiguration.Font;
        obj.color = textConfiguration.Color;
        obj.text = text;
    }
    
    ///<summary>
    ///Force unity to immediately update content transform. 
    ///</summary>
    public void UpdateContentPanel()
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}