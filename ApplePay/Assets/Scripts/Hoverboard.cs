using UnityEngine;
using UnityEngine.UI;

public class Hoverboard : MonoBehaviour
{
    [SerializeField] private ItemDescription defaultDescription;
    [SerializeField] private Text _nametag;
    [SerializeField] private Text _description;
    [SerializeField] private Text _additionalFieldObject;
    [SerializeField] private RectTransform additionalFieldList;
    public System.Collections.Generic.List<Text> InstantiatedFields = new System.Collections.Generic.List<Text>();
    private void Start() => SetDefaultDescription();
    public void SetDescription(string name, string description)
    {
        _nametag.text = name;
        _description.text = description;
    }
    public void SetDefaultDescription()
    {
        RemoveAddditionalFields();
        SetDescription(defaultDescription.Name, defaultDescription.Description);
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
    }
    public void RemoveAddditionalFields()
    {
        foreach(Text text in InstantiatedFields) Destroy(text.gameObject);
        InstantiatedFields.Clear();
    }
}
