using UnityEngine;

public abstract class Creature : Entity, IKillHandler
{
    public struct EntityState
    {
        public enum InState
        {
            Engaged,
            ///<summary>State mark for states that fully removes control of the Entity.</summary>
            Blocked
        }
        public Creature entity;
        public InState state;
        public float time;
        public bool temp;
        private System.Action cancelAction;
        public bool IsNotNull() => entity != null;
        public void CancelAction() => cancelAction?.Invoke();
        public void Remove()
        {
            entity.StateLayers.Remove(this);
            this = default(EntityState);
        }
        public EntityState(Creature entity, InState state, System.Action cancelAction, float time, bool temp)
        {
            this.entity = entity;
            this.cancelAction = cancelAction;
            this.state = state;
            this.time = time;
            this.temp = temp;
        }
    }
    private System.Collections.Generic.List<EntityState> StateLayers = new System.Collections.Generic.List<EntityState>();
    public bool IsFree() => StateLayers.Count == 0;
    public EntityState Engage(float time, System.Action cancelAction) 
    {
        EntityState state = new EntityState(this, EntityState.InState.Engaged, cancelAction, time, true);
        StateLayers.Add(state);
        return state;
    }
    public EntityState Engage(System.Action cancelAction) 
    {
        EntityState state = new EntityState(this, EntityState.InState.Engaged, cancelAction, 0.0f, false);
        StateLayers.Add(state);
        return state;
    }
    public bool IsBlocked() 
    {
        foreach(EntityState state in StateLayers) if(state.state == EntityState.InState.Blocked) return true;
        return false;
    }
    
    [HideInInspector] public EntityState.InState State;
    public EntityMovement Movement { get; set; }
    public InventorySystem InventorySystem { get; set; }
    public HealthBar HealthBar;
    public LootTable DropTable;
    public Room CurrentRoom;
    private Room oldRoom;
    public delegate void DamageCallback(ref int damage, ref DamageType damageType, ref Creature creature);
    public DamageCallback Callback;
    [SerializeField] private Color32 startColor;
    [SerializeField] private Color32 takeDamageColor;
    [SerializeField] internal float DamageInvulnerabilityDuration;
    protected float TimeSinceInvulnerability;
    [HideInInspector] public bool isDead;
    public LevelController LevelController {get; set;}
    [Header("Entity Settings")]
    public string[] Tags;
    private byte disableID;
    protected override void Awake()
    {
        LevelController = FindObjectOfType<LevelController>();
        base.Awake();
        HealthBar?.IndicatorSetup();
    }
    protected override void Start()
    {
        base.Start();
        LevelController.UpdateRoomEntityList();
        CheckTagsValidation();
        if(Movement == null) Movement = GetComponent<EntityMovement>();
        if(Movement != null) 
        {
            Movement.Entity = this;
            Movement.Rigidbody = rb;
            this.AddAttribute(
            "movementSpeed",
            new FloatRef(
            () => Movement.CurrentSpeed,
            val => Movement.CurrentSpeed = val
            ),
            Movement.CurrentSpeed);
        }
    }
    private void CollisionUpdate()
    {
        if(Movement != null)
            if(CollisionHandler.disabled && disableID == 0)
            {
                disableID = Movement.AddDisable();
            }
            else if(CollisionHandler.disabled == false && disableID != 0)
            {
                Movement.RemoveDisable(disableID);
                disableID = 0;
            }
    }
    private void CheckTagsValidation()
    {
        foreach(string tag in Tags)
        {
            if(LevelController.Tags.ContainsKey(tag) == false) Debug.LogWarning("Tag \"" + tag +"\" " + "doesn't contain any definitions in Level Controller", this);
        }
    }
    protected override void Update()
    {
        base.Update();
        CollisionUpdate();
        Invulnerability();

        ChangeRoomCheck();
        HandleStateLayers();
    }
    private void HandleStateLayers()
    {
        for(int i = 0; i < StateLayers.Count; i++)
        {
            if(StateLayers[i].temp)
            {
                if(StateLayers[i].time > 0)
                {
                    EntityState state = StateLayers[i];
                    state.time -= Time.deltaTime;
                    StateLayers[i] = state;
                }
                else StateLayers[i].Remove();
            }
        }
    }
    private void ChangeRoomCheck()
    {
        if(oldRoom != CurrentRoom)
        {
            OnRoomChanged(CurrentRoom);
        }
        oldRoom = CurrentRoom;
    }
    protected virtual void OnRoomChanged(Room room)  { }
    protected virtual void Invulnerability() 
    {
        if(TimeSinceInvulnerability < DamageInvulnerabilityDuration)
        {
            TimeSinceInvulnerability += Time.deltaTime;
            Immortal = true;
            OnInvulnerability();
        }
        else if(Immortal)
        {
            OnInvulnerabilityEnd();
            Immortal = false;
        }
    }
    protected virtual void OnInvulnerability() {}
    protected virtual void OnInvulnerabilityEnd() {}
    public override void Damage(int amount, DamageType damageType, Creature handler)
    {
        Callback.Invoke(ref amount, ref damageType, ref handler);
        base.Damage(amount, damageType, handler);
        HealthBar?.IndicatorUpdate();
    }
    public override void ChangeHealth(int amount)
    {
        base.ChangeHealth(amount);
        HealthBar?.IndicatorUpdate();
    }
    protected override void ApplyDamage(Creature handler)
    {
        base.ApplyDamage(handler);
        StartImmortality();
        SpriteRenderer.color = takeDamageColor;
        Invoke("StartColor", 0.2f);
        HealthBar?.Animator.SetTrigger("TakeDamage");
    }
    protected void StartImmortality() => TimeSinceInvulnerability = 0;
    protected override void Die(Creature killer)
    {
        DropTable?.DropLoot();
        base.Die(killer);
    }
    public virtual void OnBeforeKill(Creature entity) { }
    public virtual void OnAfterKill()
    {
        LevelController.UpdateRoomEntityList();
    }
    private void OnDestroy()
    {
        isDead = true;
    }
    private void StartColor() => SpriteRenderer.color = startColor;
}