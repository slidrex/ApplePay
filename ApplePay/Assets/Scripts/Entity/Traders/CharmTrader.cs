using UnityEngine;

public class CharmTrader : InteractiveObject
{
    public override bool HoldFoundable => false;
    [SerializeField] private GameObject cards;
    private Animator animator;
    private GameObject obj;
    private bool closed;
    private bool inTrade = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        obj = Instantiate(cards, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
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
            obj.SetActive(true);
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
            print("finish interact");
        }
        inTrade = false;
        closed = true;
        animator.SetBool("isOpen", false);
        if (obj != null) obj.SetActive(false);
    }
}
