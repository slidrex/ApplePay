using System.Collections.Generic;
using UnityEngine;
using PayWorld;
using System.Linq;
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer SpriteRenderer;
    public Dictionary<byte, byte[]> EffectBundleBuffer = new Dictionary<byte, byte[]>();
    public Dictionary<byte, EffectController.ActiveEffect> ActiveEffects = new Dictionary<byte, EffectController.ActiveEffect>(); 
    public bool Immortal;
    [SerializeField] private GameObject deathParticle, takeDamageParticle, appearParticle;
    [Header("Health")]
    public int MaxHealth = 100;
    [ReadOnly] public int CurrentHealth;
    public System.Collections.Generic.List<EntityAttribute> Attributes = new System.Collections.Generic.List<EntityAttribute>();
    public void AddAttribute(IAttributable attribute, string name) => Attributes.Add(new EntityAttribute(attribute, name));
    public EntityAttribute FindAttribute(string name)  => Attributes.First(x => x.Name == name);
    protected virtual void Awake()
    {
        if(SpriteRenderer == null) SpriteRenderer = GetComponent<SpriteRenderer>();
        Particles.InstantiateParticles(appearParticle, transform.position, Quaternion.identity, 2);
        CurrentHealth = MaxHealth;
    }
    protected virtual void Start() {}
    protected virtual void Update() => EffectsUpdate();
    public virtual void ChangeHealth(int amount) => ChangeHealth(amount, null);
    public virtual void ChangeHealth(int amount, Creature handler)
    {
        if(amount < 0 && Immortal)
            amount = 0;
        
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        if(amount < 0) ApplyDamage(handler);
    }
    protected virtual void ApplyDamage(Creature handler)
    {
        Particles.InstantiateParticles(takeDamageParticle, transform.position, Quaternion.identity, 1.5f, transform);
        if(CurrentHealth <= 0)
            Die(handler);
    }
    protected virtual void Die(Creature killer)
    {   
        if(killer != null && killer.GetComponent<IKillHandler>() != null) killer.GetComponent<IKillHandler>().OnKill(this);
        Destroy(gameObject);
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
                
                for(int j = 0; j < activeEffect.EffectProperties.Count; j++)
                {
                    PayWorld.Effect.EffectProperty property = activeEffect.EffectProperties[j];
                    UpdateEffectTick(property.StateEffect);
                    activeEffect.EffectProperties[j] = property;
                }
                
                if(ActiveEffects.ElementAt(i).Value.Endless == false)
                {
                    activeEffect.RemainTime -= Time.deltaTime;
                    if(ActiveEffects[ActiveEffects.ElementAt(i).Key].RemainTime <= 0)
                    {
                        byte id = ActiveEffects.ElementAt(i).Key;
                        PayWorld.EffectController.RemoveEffect(this, ref id);
                        return;
                    }
                }
                ActiveEffects[ActiveEffects.ElementAt(i).Key] = activeEffect;
        }
        
    }
    private void UpdateEffectTick(PayWorld.Effect.StateEffect state)
    {
        PayWorld.Effect.StateEffect.TickImplementation tick = state.TickImplement;
        if(tick.Equals(new PayWorld.Effect.StateEffect.TickImplementation())) return;
        if(tick.TimeSinceAction >= tick.TickScale)
        {
            tick.TimeSinceAction = 0;
            tick.TickAction.Invoke(this);
        }
        else tick.TimeSinceAction += Time.deltaTime;
        state.TickImplement = tick;
    }
}