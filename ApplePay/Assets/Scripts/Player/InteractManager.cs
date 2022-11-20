using UnityEngine;
using System.Linq;
public class InteractManager : MonoBehaviour
{
    private IWavedepent wavedepentComponent;
    private Animator anim;
    private EntityMovement movement;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float interactDistance;
    private InteractiveObject currentInteractObj;
    private bool InteractUpdate;
    [HideInInspector] public bool InInteract;
    private Pay.UI.IndicatorObject interactIndicator;
    private byte constraintId;
    public float HackSpeed = 1;
    public Creature entity;
    [SerializeField] private Pay.UI.UIHolder holder;
    [SerializeField] private Pay.UI.Indicator indicator;
    private void Awake()
    {
        entity = GetComponent<Creature>();
        entity.AddAttribute("hackSpeed", new FloatRef(
            () => HackSpeed, val => HackSpeed = val), HackSpeed
        );
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EntityMovement>();
    }
    public void ChangeHackSpeed(float amount) => HackSpeed += amount;
    private void Update()
    {
        InteractUpdate = false;
        if(Input.GetKey(interactKey)) Interact();
        if(InteractUpdate == false && InInteract == true)
        {
            if(currentInteractObj.InInteract) currentInteractObj.InteractEnd();
            InteractEnd();
        }
    }
    private void Interact()
    {
        if(WaveStatusCheck() == WaveStatus.InWave)
            return;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactDistance).Where(x => x.GetComponent<InteractPointer>() != null).ToArray();
        
        for(int i = 0; i < colliders.Length; i++)
        {
            for(int j = i + 1; j < colliders.Length; j++)
            {
                if(Vector2.Distance(colliders[j].transform.position, transform.position) < Vector2.Distance(colliders[i].transform.position, transform.position))
                {
                    Collider2D tempCollider = colliders[i];
                    colliders[i] = colliders[j];
                    colliders[j] = tempCollider;
                }
            }
        }
        InteractiveObject wrappedObject = null;
        foreach(Collider2D collider in colliders)
        {
            InteractiveObject current = collider.GetComponent<InteractPointer>().AttachedInteractive;
            if(current.NonInteractable == false)
            {
                wrappedObject = current;
                break;
            }
        }
        
        if(wrappedObject != null)
        {
            if(InInteract == false) 
            {
                currentInteractObj = wrappedObject;
                InteractBegin(currentInteractObj);
            }
            InteractLoop(currentInteractObj);
        }
    }
    private WaveStatus WaveStatusCheck()
    {
        wavedepentComponent = entity.GetComponent<IWavedepent>();
        if(wavedepentComponent == null) return WaveStatus.NoWave;
        return wavedepentComponent.WaveStatus;
    }
    private void InteractBegin(InteractiveObject interactiveObj)
    {
        interactiveObj.OnInteractBegin(this);
        anim.SetBool("isUnhacking", false);
        anim.SetTrigger("isHacking");
        Entity curEntity = gameObject.GetComponent<Entity>();

        PayWorld.EffectController.ActiveEffect effect = PayWorld.EffectController.AddEffect(curEntity, out constraintId, 
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.States.WeaponConstraint()),
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.States.MoveConstraint())
        );
        
        PayWorld.EffectController.AttachVisualAttrib(effect, "Interact Constrainer", "Some abilities are under constraint.", "", null);
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.FollowCanvas, indicator, out interactIndicator,
            Pay.UI.Options.Transform.StaticProperty.Position(transform.position + Vector3.up / 1.2f),
            Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 3, Vector3.one / 4, true, 0.5f)
        );
    }
    private void InteractLoop(InteractiveObject interactiveObj)
    {
        InteractUpdate = true;
        InInteract = true;
        Vector2 dist = interactiveObj.gameObject.transform.position - transform.position;
        Vector2 state = Mathf.Abs(dist.x) > Mathf.Abs(dist.y) ? Vector2.right * Mathf.Sign(dist.x) : Vector2.up * Mathf.Sign(dist.y);
        movement.SetFacingState(state, Time.deltaTime, StateParameter.MirrorHorizontal);
        interactiveObj.InteractLoop();
        movement.SetMoveMod(false);
        if(interactiveObj.GetComponent<HackSystem>() != null)
            Pay.UI.UIManager.Indicator.UpdateIndicator(interactIndicator, interactiveObj.GetComponent<HackSystem>().CurrentProgess, interactiveObj.GetComponent<HackSystem>().MaxProgress);
    }
    public void InteractEnd()
    {
        InInteract = false;
        currentInteractObj = null;
        anim.SetBool("isUnhacking", true);
        PayWorld.EffectController.RemoveEffect(entity, ref constraintId);
        Pay.UI.UIManager.RemoveUI(interactIndicator);
    }
    
}