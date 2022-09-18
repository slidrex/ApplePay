using UnityEngine;
using UnityEngine.UI;

public class Hoverboard : MonoBehaviour
{
    [SerializeField] private ItemDescription defaultDescription;
    [SerializeField] private Text _nametag;
    [SerializeField] private Text _description;
    [SerializeField] private Text _additionalFieldObject;
    [SerializeField] private Transform additionalFieldList;
    [ReadOnly] public System.Collections.Generic.List<Text> InstantiatedFields = new System.Collections.Generic.List<Text>();
    private void Start() => SetDefaultDescription();
    public void SetDescription(string name, string description)
    {
        _nametag.text = name;
        _description.text = description;
    }
    public void SetDefaultDescription()
    {
        RemoveAddditionalFields();
        _nametag.text = defaultDescription.Name;
        _description.text = defaultDescription.Description;
    }
    public void AddField(string text, Color color)
    {
        GameObject obj = Instantiate(_additionalFieldObject.gameObject, transform.position, Quaternion.identity);
        Vector2 sourceScale = obj.transform.localScale;
        obj.transform.SetParent(additionalFieldList);
        obj.transform.localScale = sourceScale;
        Text _text = obj.GetComponent<Text>();
        InstantiatedFields.Add(_text);
        _text.text = text;
        _text.color = color;
    }
    public void RemoveAddditionalFields()
    {
        for(int i = 0 ; i < InstantiatedFields.Count; i++)
        {
            Destroy(InstantiatedFields[i].gameObject);
            InstantiatedFields.RemoveAt(i);
        }
    }
}
