using UnityEngine;
public abstract class InteractiveObject : MonoBehaviour
{
    public InteractPointer InteractPointer;
    public Pay.UI.Indicator indicator;
    public bool NonInteractable;
    public bool InInteract;
    protected InteractManager InteractEntity;
    private void Awake() => InteractPointer.AttachedInteractive = this;
    public void InteractBegin(InteractManager interactEntity) => OnInteractBegin(interactEntity);
    public void InteractLoop() => OnInteractLoop();
    public void InteractEnd()
    {
        OnInteractEnd();
        InInteract = false;
        InteractEntity = null;
    }
    protected void InteractAction()
    {
        OnInteractAction();
        InteractEnd();
    }
    public virtual void OnInteractBegin(InteractManager interactEntity)
    {
        InInteract = true;
        InteractEntity = interactEntity;
    }
    protected virtual void OnInteractLoop() { }
    protected virtual void OnInteractEnd() { }
    protected virtual void OnInteractAction() { }
}