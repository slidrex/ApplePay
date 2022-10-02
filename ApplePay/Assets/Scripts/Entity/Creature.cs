using UnityEngine;

public abstract class Creature : Entity
{
    [HideInInspector] public InventorySystem InventorySystem;
    public HealthBar HealthBar;
    public LootTable DropTable;
    [ReadOnly] public Room CurrentRoom;
    [SerializeField] private Color32 startColor;
    [SerializeField] private Color32 takeDamageColor;
    [Header("After Take Damage Immortality")]
    [SerializeField] internal float ImmortalityDuration;
    private float timeSinceImmortality;
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
        if(timeSinceImmortality < ImmortalityDuration) 
        {
            timeSinceImmortality += Time.deltaTime;
            Immortal = true;
        } else if(Immortal) Immortal = false;

    }

    public override void ChangeHealth(int changeAmount)
    {
        base.ChangeHealth(changeAmount);
        HealthBar?.IndicatorUpdate();
    }
    protected override void ApplyDamage(Creature handler)
    {
        base.ApplyDamage(handler);
        StartImmortality(ImmortalityDuration);
        SpriteRenderer.color = takeDamageColor;
        Invoke("StartColor", 0.2f);
        HealthBar?.Animator.SetTrigger("TakeDamage");
    }
    protected override void OnEvasion()
    {
        base.OnEvasion();
        StartImmortality(ImmortalityDuration/2);
    }
    protected void StartImmortality(float time) => timeSinceImmortality = ImmortalityDuration - time;
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