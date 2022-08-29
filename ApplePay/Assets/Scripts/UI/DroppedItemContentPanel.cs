using UnityEngine;

public class DroppedItemContentPanel : ContentPanel
{
    [SerializeField] private UnityEngine.UI.Text header;
    [SerializeField] private UnityEngine.UI.Text description;
    public void SetHeader(string text) => header.text = text;
    public void SetDescription(string text) => description.text = text;
}
