using UnityEngine.EventSystems;
using UnityEngine;

public class HoverableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isPointerEntered;
    protected virtual void Update()
    {
        if(_isPointerEntered) OnPointer();
    }
    public virtual void OnPointerEnter(PointerEventData pointerData) =>  _isPointerEntered = true;
    
    public virtual void OnPointerExit(PointerEventData pointerData) => _isPointerEntered = false;
    public virtual void OnPointer() {}
}
