using UnityEngine;

public class AbilityGridCell : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image currentImage;
    [SerializeField] private UnityEngine.UI.Image fillMask;
    public void SetImage(UnityEngine.Sprite sprite)
    {
        currentImage.sprite = sprite;
    }
    public void SetMaskFillAmount(float value)
    {
        fillMask.fillAmount = value;
    }
}
