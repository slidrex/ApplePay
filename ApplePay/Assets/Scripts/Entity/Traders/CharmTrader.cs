using UnityEngine;

public class CharmTrader : InteractiveObject
{
    [SerializeField] private GameObject cards;
    private Animator animator;
    private PlayerEntity player;
    private GameObject obj;
    private bool closed;
    private bool inTrade = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerEntity>();
        obj = Instantiate(cards, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
    }
    public override void OnInteractZoneLeft(InteractManager interactEntity)
    {
        if(closed == false) TraderClose(interactEntity, false); 
        interactEntity.InInteract = false;
    }
    public override void OnInteractKeyDown(InteractManager interactEntity)
    {
        if(inTrade == false && player.IsFree())
        {
            player.Engage();
            closed = false;
            inTrade = true;
            obj.SetActive(true);
            animator.SetBool("isOpen", true);
        }
            else if(inTrade == true)
            {
                TraderClose(interactEntity, true);
            }
    }
    private void TraderClose(InteractManager entity, bool stayInteract)
    {
        if(inTrade)
        {
            entity.FinishInteract(this, stayInteract);
        }
        inTrade = false;
        closed = true;
        animator.SetBool("isOpen", false);
        if (obj != null) obj.SetActive(false);
    }
}
