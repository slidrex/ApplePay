using UnityEngine;

public abstract class Creature : Entity
{
    [HideInInspector] public InventorySystem InventorySystem;
    public HealthBar HealthBar;
    public LootTable DropTable;
    [ReadOnly] public Room CurrentRoom;
    [SerializeField] private Color32 startColor;
    [SerializeField] private Color32 takeDamageColor;
    [SerializeField] internal float DamageInvulnerabilityDuration;
    protected float TimeSinceInvulnerability;
    [HideInInspector] public bool isDead;
    [Header("Entity Settings")]
    public System.Collections.Generic.List<Creature> Hostiles = new System.Collections.Generic.List<Creature>();
    protected override void Awake()
    {
        base.Awake();
        HealthBar?.IndicatorSetup();
    }
    protected override void Start()
    {
        base.Start();
        GlobalEventManager.UpdateMobLists();
    }
    protected override void Update()
    {
        base.Update();
        Invulnerability();
    }
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
    private void OnDestroy()
    {
        isDead = true;
        GlobalEventManager.UpdateMobLists();
    }
    private void StartColor() => SpriteRenderer.color = startColor;
}