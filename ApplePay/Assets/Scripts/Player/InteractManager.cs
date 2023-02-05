using UnityEngine;
using System.Linq;

public class InteractManager : MonoBehaviour
{
    public Animator anim {get; set;}
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float interactDistance;
    public bool InInteract { get; private set; }
    private byte constraintId;
    public float HackSpeed = 1;
    public Creature entity {get; set;}
    private Pay.UI.UIHolder holder;
    public Pay.UI.Indicator DefaultIndicator;
    [HideInInspector] public Pay.UI.IndicatorObject indicatorObject;
    private CurrentInteractObject currentInteractObject;
    private int currentInteractObjectIndex;
    public System.Collections.Generic.List<CurrentInteractObject> InteractObjects = new System.Collections.Generic.List<CurrentInteractObject>();
    public InteractHint Hint;
    private InteractHint hint;
    private bool hintSpawned;
    private InteractiveObject hintedObject;
    private Creature.EntityState engagingStatus;
    public Transform currentTransform;
    private void Awake()
    {
        entity = GetComponent<Creature>();
        anim = GetComponent<Animator>();
        holder = entity.GetComponent<IUIHolder>()?.GetHolder();
        entity.AddAttribute("hackSpeed", new FloatRef(
            () => HackSpeed, val => HackSpeed = val), HackSpeed
        );
    }
    private void Start()
    {
        if(entity != null) currentTransform = entity.transform;
        else currentTransform = transform;
    }
    public void ChangeHackSpeed(float amount) => HackSpeed += amount;
    private bool IsValidate()
    {
        if(entity.IsFree() == false) return false;
        
        return true;
    }
    private void Update()
    {
        bool hasInteractObjects = UpdatePotentialInteractiveObjectList();
        
        InteractiveObject closestObject = null;
        if(hasInteractObjects) closestObject = GetNearestInteractiveObject(out int id).interactiveObject;
        
        if(hintSpawned && (entity.IsFree() == false || closestObject == null || closestObject != hintedObject))
        {
            RemoveInteractHint();
        }
        else if(hintSpawned == false && entity.IsFree() && closestObject != null)
        {
            CreateInteractHint(closestObject.InteractPointer);
            hintedObject = closestObject;
        }
        if(hasInteractObjects && ((currentInteractObject.interactiveObject != null && currentInteractObject.interactiveObject.IsValidInteract(this)) || (currentInteractObject.interactiveObject == null && closestObject.IsValidInteract(this))))
        {
            HandleInteractInputs();
        }
    }
    private void HandleInteractInputs()
    {
        if(Input.GetKeyDown(interactKey))
        {
            if(currentInteractObject.interactiveObject == null && InInteract == false && entity.IsFree())
            {
                currentInteractObject = GetNearestInteractiveObject(out currentInteractObjectIndex);
                InInteract = true;
            }
            InteractObjects[currentInteractObjectIndex].interactiveObject.OnInteractKeyDown(this);
        }
        if(Input.GetKey(interactKey) && InInteract == true)
        {
            bool firstInteraction = false;
            if(currentInteractObject.interactInitiated == false)
            {
                firstInteraction = true;
                currentInteractObject.interactInitiated = true;
                InteractObjects[currentInteractObjectIndex] = currentInteractObject;
            }
            InteractObjects[currentInteractObjectIndex].interactiveObject.OnInteractKeyHolding(this, firstInteraction);
        }
        if(Input.GetKeyUp(interactKey) && InInteract == true)
        {
            if(InteractObjects[currentInteractObjectIndex].interactInitiated == true) InteractObjects[currentInteractObjectIndex].interactiveObject.OnInteractKeyUp(this);
        }

    }
    private CurrentInteractObject GetNearestInteractiveObject(out int associatedIndex) 
    {
        associatedIndex = 0;
        float sqrMinDist = Vector2.SqrMagnitude(InteractObjects[0].collider.transform.position - currentTransform.position);
        for(int i = 1; i < InteractObjects.Count; i++)
        {
            float dist = Vector2.SqrMagnitude(InteractObjects[i].collider.transform.position - currentTransform.position);
            if(dist < sqrMinDist)
            {
                sqrMinDist = dist;
                associatedIndex = i;
            }
        }
        
        return InteractObjects[associatedIndex];
    }
    
    private bool UpdatePotentialInteractiveObjectList()
    {
        InteractPointer[] colliders = Physics2D.OverlapCircleAll(currentTransform.position, interactDistance).Select(x => x.GetComponent<InteractPointer>()).ToArray();
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i] != null)
                for(int j = i + 1; j < colliders.Length; j++)
                {
                    if(colliders[i] == colliders[j]) colliders[j] = null;
                }
        }
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i] == null) continue;
            
            Collider2D currentCollider = colliders[i].rangeCollider;
            bool isNew = true;
            foreach(CurrentInteractObject currentInteractObject in InteractObjects)
            {
                if(currentInteractObject.collider == currentCollider)
                {
                    isNew = false;
                    break;
                }
            }
            if(isNew == true)
            {
                CurrentInteractObject curObj = new CurrentInteractObject(currentCollider.gameObject.GetComponent<InteractPointer>().AttachedInteractive, currentCollider.transform, currentCollider);
                curObj.interactiveObject.OnInteractZoneEntered(this);
                curObj.collider = currentCollider;
                InteractObjects.Add(curObj);
            }
        }
        
        for(int i = 0; i < InteractObjects.Count; i++)
        {
            CurrentInteractObject currentKey = InteractObjects[i];
            bool valid = false;
            foreach(InteractPointer pointer in colliders)
            {
                if(pointer != null && pointer.rangeCollider == currentKey.collider && pointer.AttachedInteractive.IsValidInteract(this) == true) 
                {
                    valid = true;
                    break;
                }
            }
            if(valid == false)
            {
                InteractObjects[i].interactiveObject.OnInteractZoneLeft(this);
                InteractObjects.RemoveAt(i);
            }
        }
        return InteractObjects.Count > 0;
        
    }
    public void SetBlockedState(InteractiveObject interactiveObj)
    {
        SetInteractState(interactiveObj);
        
        PayWorld.EffectController.ActiveEffect effect = PayWorld.EffectController.AddEffect(entity, out constraintId, 
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.WeaponConstraint()),
            new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.MoveConstraint())
        );
        
        PayWorld.EffectController.AttachVisualAttrib(effect, "Interact Constrainer", "Some abilities are under constraint.", "", null);
        
    }
    public void FinishInteract(InteractiveObject interactiveObject)
    {
        
        int index = InteractObjects.IndexOf(InteractObjects.Find(x => x.interactiveObject == interactiveObject));
        CurrentInteractObject obj = InteractObjects[index];
        obj.interactInitiated = false;
        InteractObjects[index] = obj;

        SetDefaultInteractState();
    }
    private void SetDefaultInteractState()
    {
        currentInteractObject = new CurrentInteractObject();
        currentInteractObjectIndex = 0;
        
        engagingStatus.Remove();
        if(indicatorObject.GetObject() != null) RemoveIndicator();
        if(constraintId != 0)
            PayWorld.EffectController.RemoveEffect(entity, ref constraintId);
        InInteract = false;
    }
    private void CancelInteract()
    {
        SetDefaultInteractState();
        for(int i = 0; i < InteractObjects.Count; i++)
        {
            CurrentInteractObject current = InteractObjects[i];
            current.interactInitiated = false;
            InteractObjects[i] = current;
        }
    }
    public void CreateIndicator(Pay.UI.Indicator indicator)
    {
        indicatorObject = Pay.UI.UIManager.Indicator.CreateIndicator(holder, holder.FollowCanvas, indicator,
            Pay.UI.Options.Transform.StaticProperty.Position(currentTransform.position + Vector3.up / 1.2f),
            Pay.UI.Options.Transform.DynamicProperty.LocalScale(Vector3.one / 4, Vector3.one / 4.5f, true, 0.1f)
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
    public void SetInteractState(InteractiveObject interactiveObj)
    {
        InInteract = true;
        engagingStatus = entity.Engage(CancelInteract);
        Vector2 dist = interactiveObj.gameObject.transform.position - transform.position;
        Vector2 state = Mathf.Abs(dist.x) > Mathf.Abs(dist.y) ? Vector2.right * Mathf.Sign(dist.x) : Vector2.up * Mathf.Sign(dist.y);
        entity.Movement.SetFacing(state, Time.deltaTime);
        
        entity.Movement.SetMoveMod(false);
    }
    private void CreateInteractHint(InteractPointer pointer)
    {
        hint = Instantiate<InteractHint>(Hint);
        Pay.UI.UIManager.Image.RegisterImage(holder, hint.image,
            Pay.UI.Options.Transform.StaticProperty.LocalScale(Vector3.one * 0.1f),
            Pay.UI.Options.Transform.StaticProperty.Position(pointer.hintPosition.position)
        );
        hint.text.text = interactKey.ToString();
        hintSpawned = true;
    }
    private void RemoveInteractHint()
    {
        hint.DisappearInteractHint();
        hintSpawned = false;
    }
    [System.Serializable]
    public struct CurrentInteractObject
    {
        public Transform transform;
        public Collider2D collider;
        public InteractiveObject interactiveObject;
        public bool interactInitiated;
        public bool enteredZone;
        public CurrentInteractObject(InteractiveObject interactiveObject, Transform transform, Collider2D collider)
        {
            this.collider = collider;
            this.interactiveObject = interactiveObject;
            enteredZone = false;
            interactInitiated = false;
            this.transform = transform;
        }
    }
}