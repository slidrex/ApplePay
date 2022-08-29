using UnityEngine;

public class EffectPanel : ContentPanel
{
    [SerializeField] private UnityEngine.UI.Text headerField;
    [SerializeField] private UnityEngine.UI.Text mainField;
    public void SetHeader(string text) => SetText(headerField, text);
    public void SetMain(string text) => SetText(mainField, text);
    private void SetText(UnityEngine.UI.Text wrappedText, string text)
    {
        wrappedText.text = text;
        UpdateContentPanel();
    }
}