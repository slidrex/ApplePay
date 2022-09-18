using UnityEngine;
using UnityEngine.UI;
public class Hoverboard : MonoBehaviour
{
    [SerializeField] private ItemDescription defaultDescription;
    [SerializeField] private Text _nametag;
    [SerializeField] private Text _description;
    private void Start() => SetDefaultDescription();
    public void SetDescription(string name, string description)
    {
        _nametag.text = name;
        _description.text = description;
    }
    public void SetDefaultDescription()
    {
        _nametag.text = defaultDescription.Name;
        _description.text = defaultDescription.Description;
    }
}
