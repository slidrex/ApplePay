using UnityEngine;
using System.Linq;
public class InteractManager : MonoBehaviour
{
    private IWavedepent wavedepentComponent;
    public Animator anim {get; set;}
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float interactDistance;
    private InteractiveObject currentInteractObj;
    private bool InteractUpdate;
    [HideInInspector] public bool InInteract;
    private byte constraintId;
    public float HackSpeed = 1;
    public Creature entity {get; set;}
    [SerializeField] private Pay.UI.UIHolder holder;
    public Pay.UI.Indicator DefaultIndicator;
    [HideInInspector] public Pay.UI.IndicatorObject indicatorObject;
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
    }
    public void ChangeHackSpeed(float amount) => HackSpeed += amount;
    private void Update()
    {
        InteractUpdate = false;
        if(Input.GetKeyDown(interactKey) && IsValidate()) Interact(true);
        if(Input.GetKey(interactKey) && IsValidate()) Interact(false);
        if(InteractUpdate == false && InInteract == true)
        {
            if(currentInteractObj.InInteract) currentInteractObj.InteractEnd(this);
            InteractEnd();
        }
    }
    private bool IsValidate()
    {
        if(InInteract == false)
        {
            if(entity.IsFree() == false) return false;
        }
        return true;
    }
    private void Interact(bool repeat)
    {
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
        
        if(wrappedObject != null && wrappedObject.BeforeInteractBegin(this))
        {
            if(InInteract == false) 
            {
                currentInteractObj = wrappedObject;
                
                InteractBegin(currentInteractObj);
                
            }
            InteractLoop(currentInteractObj);
        }
    }
    private void InteractBegin(InteractiveObject interactiveObj)
    {
        entity.Engage();
        interactiveObj.OnInteractBegin(this);
        
        
        PayWorld.EffectController.ActiveEffect effect = PayWorld.EffectController.AddEffect(entity, out constraintId, 
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.WeaponConstraint()),
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.MoveConstraint())
        );
        
        PayWorld.EffectController.AttachVisualAttrib(effect, "Interact Constrainer", "Some abilities are under constraint.", "", null);
        
    }
    public void CreateIndicator(Pay.UI.Indicator indicator)
    {
        Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.FollowCanvas, indicator, out indicatorObject,
            Pay.UI.Options.Transform.StaticProperty.Position(transform.position + Vector3.up / 1.2f),
            Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 3, Vector3.one / 4, true, 0.5f)
        );
    }
    public void UpdateIndicator(float currentAmount, float maxAmount)
    {
        Pay.UI.UIManager.Indicator.UpdateIndicator(indicatorObject, currentAmount, maxAmount);
    }
    public void RemoveIndicator()
    {
        Pay.UI.UIManager.RemoveUI(indicatorObject);
    }
    private void InteractLoop(InteractiveObject interactiveObj)
    {
        InteractUpdate = true;
        InInteract = true;
        Vector2 dist = interactiveObj.gameObject.transform.position - transform.position;
        Vector2 state = Mathf.Abs(dist.x) > Mathf.Abs(dist.y) ? Vector2.right * Mathf.Sign(dist.x) : Vector2.up * Mathf.Sign(dist.y);
        entity.Movement.SetFacingState(state, Time.deltaTime, StateParameter.MirrorHorizontal);
        interactiveObj.InteractLoop(this);
        entity.Movement.SetMoveMod(false);
    }
    public void FinishInteract()
    {
        InteractEnd();
        InInteract = false;
    }
    private void InteractEnd()
    {
        InInteract = false;
        currentInteractObj = null;
        entity.UnEngage();
        if(indicatorObject.GetObject() != null) RemoveIndicator();
        PayWorld.EffectController.RemoveEffect(entity, ref constraintId);
    }
    
}