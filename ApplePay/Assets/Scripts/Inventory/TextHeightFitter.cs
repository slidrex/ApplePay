using UnityEngine;

[ExecuteAlways]

public class TextHeightFitter : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private UnityEngine.UI.Text _text;

    private void Update()
    {
        if (!_text) _text = GetComponent<UnityEngine.UI.Text>();
        if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();

        var desiredHeight = _text.preferredHeight;
        var size = _rectTransform.sizeDelta;
        size.y = desiredHeight;
        _rectTransform.sizeDelta = size;
    }
}