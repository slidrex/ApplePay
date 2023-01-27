using UnityEngine;

public class CharmTrader : InteractiveObject
{
    [SerializeField] private GameObject cardCanvas;
    private Animator animator;
    private GameObject currentCardCanvas;
    private bool closed;
    private bool inTrade = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentCardCanvas = Instantiate(cardCanvas, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
        currentCardCanvas.SetActive(false);
    }
    public override void OnInteractZoneLeft(InteractManager interactEntity)
    {
        if(closed == false) TraderClose(interactEntity);
    }
    public override void OnInteractKeyDown(InteractManager interactEntity)
    {
        if(inTrade == false && interactEntity.entity.IsFree())
        {
            interactEntity.SetBlockedState(this);
            closed = false;
            inTrade = true;
            currentCardCanvas.SetActive(true);
            animator.SetBool("isOpen", true);
        }
        else if(inTrade == true)
        {
            TraderClose(interactEntity);
        }
    }
    private void TraderClose(InteractManager entity)
    {
        if(inTrade)
        {
            entity.FinishInteract(this);
        }
        inTrade = false;
        closed = true;
        animator.SetBool("isOpen", false);
        if (currentCardCanvas != null) currentCardCanvas.SetActive(false);
    }
}
