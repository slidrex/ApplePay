using UnityEngine;

public abstract class Creature : Entity, IKillHandler
{
    public InventorySystem InventorySystem { get; set; }
    public HealthBar HealthBar;
    public LootTable DropTable;
    public Room CurrentRoom;
    private Room oldRoom;
    [SerializeField] private Color32 startColor;
    [SerializeField] private Color32 takeDamageColor;
    [SerializeField] internal float DamageInvulnerabilityDuration;
    protected float TimeSinceInvulnerability;
    [HideInInspector] public bool isDead;
    public LevelController LevelController {get; set;}
    [Header("Entity Settings")]
    public string[] Tags;
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
        Invulnerability();
        ChangeRoomCheck();
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
    public virtual void OnBeforeKill(Creature entity)
    {
        
    }
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