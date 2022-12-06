using UnityEngine;
using Pay.UI;

public class KeyHoldingHack : HackSystem
{
    [SerializeField] private bool saveProgess;
    private const float OpenTime = 0.35f;
    protected override void OnInteractLoop(InteractManager interactEntity)
    {
        base.OnInteractLoop(interactEntity);
        if(CurrentProgress < MaxProgress)
        {
            CurrentProgress += Time.deltaTime * interactEntity.HackSpeed;
            interactEntity.UpdateIndicator(CurrentProgress, MaxProgress);
            if(CurrentProgress >= MaxProgress)
            {
                interactEntity.FinishInteract();
                if(isUnlocked == false) OnAfterHack(interactEntity);
                InteractEnd(interactEntity, true);
                
            }
        }
    }
    public override void OnInteractBegin(InteractManager interactEntity)
    {
        base.OnInteractBegin(interactEntity);
        interactEntity.CreateIndicator(interactEntity.DefaultIndicator);
    }
    protected override void OnInteractEnd(InteractManager interactEntity)
    {
        base.OnInteractEnd(interactEntity);
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