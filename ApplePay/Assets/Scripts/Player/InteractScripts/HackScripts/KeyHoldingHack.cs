using UnityEngine;

public class KeyHoldingHack : HackSystem
{
    [SerializeField] private bool saveProgess;
    private const float OpenTime = 0.35f;
    public override void OnInteractKeyHolding(InteractManager interactEntity, bool first)
    {
        if(first)
        {
            if(interactEntity.entity.IsFree())
            {
            interactEntity.CreateIndicator(interactEntity.DefaultIndicator);
            OnInteractBegin(interactEntity);

            }
        }
        
        if(CurrentProgress < MaxProgress)
        {
            CurrentProgress += Time.deltaTime * interactEntity.HackSpeed;
            
            interactEntity.UpdateIndicator(CurrentProgress, MaxProgress);
            if(CurrentProgress >= MaxProgress)
            {
                interactEntity.FinishInteract(this, false);
                if(isUnlocked == false) OnAfterHack(interactEntity);

                OnInteractInterruption(interactEntity);
            }
        }
    }
    protected virtual void OnInteractBegin(InteractManager interactEntity)
    {
        interactEntity.SetBlockedState(this);
    }
    public override void OnInteractKeyUp(InteractManager interactEntity)
    {
        OnInteractInterruption(interactEntity);
    }
    protected virtual void OnInteractInterruption(InteractManager interactEntity)
    {
        if(interactEntity.InInteract == false) return;
        interactEntity.FinishInteract(this, false);
        interactEntity.RemoveIndicator();
        if(!saveProgess) CurrentProgress = 0;

    }
    protected override void OnInteractAction(InteractManager interactEntity)
    {
        base.OnInteractAction(interactEntity);
        CurrentProgress = 0;
    }
    public override void OnUnlock()
    {
        base.OnUnlock();
        ResetOpenTime();
    }
    public void ResetOpenTime() => MaxProgress = OpenTime;
}