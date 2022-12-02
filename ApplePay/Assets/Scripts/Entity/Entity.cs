using System.Collections.Generic;
using UnityEngine;
using PayWorld;
using System.Linq;

public abstract class Entity : MonoBehaviour
{
    public EntityMovement Movement { get; set; }
    protected SpriteRenderer SpriteRenderer;
    public Dictionary<byte, byte[]> EffectBundleBuffer = new Dictionary<byte, byte[]>();
    public Dictionary<byte, EffectController.ActiveEffect> ActiveEffects = new Dictionary<byte, EffectController.ActiveEffect>(); 
    public bool Immortal;
    public bool isKnockable {get => !Immortal;}
    [SerializeField] private GameObject deathParticle, takeDamageParticle, appearParticle;
    [Header("Health")]
    public int MaxHealth = 100;
    public int CurrentHealth {get ;set;}
    [SerializeField] private float evasionRate;
    [SerializeField] private float magicResistance;
    [SerializeField] private GameObject evasionEffect;
    public System.Collections.Generic.Dictionary<string, EntityAttribute> Attributes = new System.Collections.Generic.Dictionary<string, EntityAttribute>();
    public PayCollisionHandler CollisionHandler;
    private byte disableID;
    
    protected virtual void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CollisionHandler.rb = rb;
        if(Movement == null) Movement = GetComponent<EntityMovement>();
        if(Movement != null) 
        {
            Movement.Entity = this;
            Movement.Rigidbody = rb;
        }
        if(SpriteRenderer == null) SpriteRenderer = GetComponent<SpriteRenderer>();
        Particles.InstantiateParticles(appearParticle, transform.position, Quaternion.identity, 2);
        CurrentHealth = MaxHealth;
        AttributesSetup();
    }
    protected virtual void Start() { }
    private void AttributesSetup()
    {
        this.AddAttribute("maxHealth", new FloatRef(
            () => MaxHealth,
            val => MaxHealth = (int)val
        ), MaxHealth);
        this.AddAttribute("evasion", new FloatRef(
            () => evasionRate,
            val => evasionRate = val
        ), 0f);
        this.AddAttribute("magicResistance", new FloatRef(
            () => magicResistance,
            val => magicResistance = val
        ), 0f);
        if(Movement != null)
            this.AddAttribute(
            "movementSpeed",
            new FloatRef(
            () => Movement.CurrentSpeed,
            val => Movement.CurrentSpeed = val
            ),
            Movement.CurrentSpeed);
    }
    protected virtual void Update()
    {
        CollisionHandler.OnUpdate();
        
        CollisionUpdate();
        EffectsUpdate();
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
    public virtual void Damage(int amount, DamageType damageType, Creature handler)
    {
        bool evaded = Random.Range(0, 1f) < evasionRate && damageType == DamageType.Physical;
        if(evaded) OnEvasion();

        float fixedMagicalDamage = 1 - Mathf.Clamp(magicResistance, Mathf.NegativeInfinity, 1);
        if(damageType == DamageType.Magical) amount = (int)((float)amount * fixedMagicalDamage);
        if(Immortal == false && evaded == false)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth -amount, 0, MaxHealth);
            ApplyDamage(handler);
        }
        if(CurrentHealth <= 0) Die(handler);
    }
    protected virtual void OnEvasion()
    {
        PayWorld.Particles.InstantiateParticles(evasionEffect, transform.position, Quaternion.identity, 0.25f, transform);
    }
    public virtual void Heal(int amount, Creature handler)
    {
        ChangeHealth(amount);
    }
    public virtual void ChangeHealth(int amount) 
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        if(CurrentHealth <= 0) Die(null);
    }
    protected virtual void ApplyDamage(Creature handler)
    {
        Particles.InstantiateParticles(takeDamageParticle, transform.position, Quaternion.identity, 1.5f, transform);
    }
    protected virtual void Die(Creature killer)
    {   
        IKillHandler killHandler = killer?.GetComponent<IKillHandler>();
        
        if(killer != null && killHandler != null) 
        {
            killer.GetComponent<IKillHandler>().OnBeforeKill(killer);
            StaticCoroutine.BeginCoroutine(OnAfterKill(killHandler));
        }
        Destroy(gameObject);
    }
    private System.Collections.IEnumerator OnAfterKill(IKillHandler handler)
    {
        yield return new WaitForEndOfFrame();
        
        handler.OnAfterKill();
    }
    protected void EffectsUpdate()
    {
        for(int i = 0; i < ActiveEffects.Count; i++)
        {
                EffectController.ActiveEffect emptyEffect = new EffectController.ActiveEffect();
                if(ActiveEffects.ElementAt(i).Value.Equals(emptyEffect))
                {
                    ActiveEffects.Remove(ActiveEffects.ElementAt(i).Key);
                    break;
                }
                
                EffectController.ActiveEffect activeEffect = ActiveEffects[ActiveEffects.ElementAt(i).Key];
                
                for(int j = 0; j < activeEffect.EffectProperties.Length; j++)
                {
                    PayWorld.Effect.EffectProperty property = activeEffect.EffectProperties[j];
                    UpdateEffectTick(property.EffectAction);
                    activeEffect.EffectProperties[j] = property;
                }
                
                if(ActiveEffects.ElementAt(i).Value.Endless == false)
                {
                    activeEffect.RemainTimeSourceValue -= Time.deltaTime * (activeEffect.RemainTimeSourceValue / activeEffect.ResultRemainTime);
                    
                    if(ActiveEffects[ActiveEffects.ElementAt(i).Key].RemainTimeSourceValue <= 0)
                    {
                        byte id = ActiveEffects.ElementAt(i).Key;
                        EffectController.RemoveEffect(this, ref id);
                        return;
                    }
                }
                ActiveEffects[ActiveEffects.ElementAt(i).Key] = activeEffect;
        }
        
    }
    private void UpdateEffectTick(PayWorld.Effect.EffectAction state)
    {
        PayWorld.Effect.EffectAction.TickImplementation tick = state.TickImplement;
        if(tick.Equals(new PayWorld.Effect.EffectAction.TickImplementation())) return;
        if(tick.TimeSinceAction >= tick.TickScale)
        {
            tick.TimeSinceAction = 0;
            tick.TickAction.Invoke(this);
        }
        else tick.TimeSinceAction += Time.deltaTime;
        state.TickImplement = tick;
    }
}
public enum DamageType
{
    Physical,
    Magical,
    Pure
}