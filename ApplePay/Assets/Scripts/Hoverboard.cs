using UnityEngine;
using UnityEngine.UI;

public class Hoverboard : MonoBehaviour
{
    [SerializeField] private ItemDescription defaultDescription;
    [SerializeField] private Text _header;
    [SerializeField] private Text _additionalFieldObject;
    [SerializeField] private RectTransform additionalFieldList;
    private RectTransform rectTransform;
    public System.Collections.Generic.List<Text> InstantiatedFields = new System.Collections.Generic.List<Text>();
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        SetDefaultDescription();
    }
    public void SetDescription(string description) => AddField(description, Color.white);
    public void SetHeader(string text, Color color)
    {
        _header.text = text;
        _header.color = color;
    }
    public void SetDefaultDescription()
    {
        RemoveAddditionalFields();
        SetDescription(defaultDescription.Description);
        SetHeader(defaultDescription.Name, Color.white);
    }
    public void AddField(string text, Color color)
    {
        Text obj = Instantiate(_additionalFieldObject, transform.position, Quaternion.identity);
        InstantiatedFields.Add(obj);
        Vector2 sourceScale = obj.transform.localScale;
        obj.transform.SetParent(additionalFieldList);
        obj.transform.localScale = sourceScale;
        
        
        obj.text = text;
        obj.color = color;
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(additionalFieldList);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    public void RemoveAddditionalFields()
    {
        foreach(Text text in InstantiatedFields) Destroy(text.gameObject);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        InstantiatedFields.Clear();
    }
}
