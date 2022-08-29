using UnityEngine;

public class KeyHoldingHack : HackSystem
{
    [SerializeField] private bool saveProgess;
    private const float OpenTime = 0.35f;
    protected override void OnInteractLoop()
    {
        base.OnInteractLoop();
        if(CurrentProgess < MaxProgress)
        {
            CurrentProgess += Time.deltaTime * InteractEntity.HackSpeed;
            if(CurrentProgess >= MaxProgress)
            {
                if(InteractEntity.InInteract) InteractEntity.InteractEnd();
                if(isUnlocked == false) AfterHack();
                InteractAction();
                
            }
        }
    }
    protected override void OnInteractEnd()
    {
        base.OnInteractEnd();
        if(!saveProgess) CurrentProgess = 0;
    }
    protected override void OnInteractAction()
    {
        base.OnInteractAction();
        CurrentProgess = 0;
    }
    public override void OnUnlock()
    {
        base.OnUnlock();
        ResetOpenTime();
    }
    public void ResetOpenTime() => MaxProgress = OpenTime;
}