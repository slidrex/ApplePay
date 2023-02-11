public class InventoryUIPage : UnityEngine.MonoBehaviour
{
    public UnityEngine.RectTransform Transform;
    public UnityEngine.Canvas AttachedCanvas { get; private set; }
    public void AttachCanvas(UnityEngine.Canvas canvas)
    {
        AttachedCanvas = canvas;
    }
}
