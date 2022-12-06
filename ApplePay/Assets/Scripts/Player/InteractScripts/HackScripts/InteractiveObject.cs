using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public InteractPointer InteractPointer;
    public bool NonInteractable;
    public bool InInteract;
    private void Awake() => InteractPointer.AttachedInteractive = this;
    public void InteractBegin(InteractManager interactEntity) => OnInteractBegin(interactEntity);
    public void InteractLoop(InteractManager interactEntity) => OnInteractLoop(interactEntity);
    ///<summary>Calls before entity interacts. Returns validation (if return false - entity's interaction would be interrupted.)</summary>
    public virtual bool BeforeInteractBegin(InteractManager interactEntity)
    {
        return true;
    }
    public void InteractEnd(InteractManager interactEntity, bool success = false)
    {
        if(success) OnInteractAction(interactEntity);
        OnInteractEnd(interactEntity);
        InInteract = false;
    }
    ///<summary>Calls if entity began interacting with Downed Interact Button</summary>
    public virtual void OnInteractBeginDown(InteractManager interactEntity)
    {

    }
    ///<summary>Calls if entity began interacting (works repeatedly).</summary>
    public virtual void OnInteractBegin(InteractManager interactEntity)
    {
        InInteract = true;
    }
    protected virtual void OnInteractLoop(InteractManager interactEntity) { }
    protected virtual void OnInteractEnd(InteractManager interactEntity) { }
    protected virtual void OnInteractAction(InteractManager interactEntity) { }
}