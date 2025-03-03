using UnityEngine;

[RequireComponent(typeof(InteractPointer))]

public abstract class InteractiveObject : MonoBehaviour
{
    ///<summary>Mark that object can be interacted if entity holds key</summary>
    public abstract bool HoldFoundable {get;}
    public InteractPointer InteractPointer;
    private void Awake() => InteractPointer.AttachedInteractive = this;
    public virtual void OnInteractZoneEntered(InteractManager interactEntity) { }
    public virtual void OnInteractZoneLeft(InteractManager interactEntity) { }
    ///<summary>Calls when interact entity presses interact key down. </summary>
    public virtual void OnInteractKeyDown(InteractManager interactEntity) { }
    ///<summary>Calls when interact entity holding interact key. </summary>
    public virtual void OnInteractKeyHolding(InteractManager interactEntity, bool firstInteraction) { }
    ///<summary>Calls when interact entity unpresses interact key. </summary>
    public virtual void OnInteractKeyUp(InteractManager interactEntity) { }
    protected virtual void OnInteractAction(InteractManager interactEntity) { }
}