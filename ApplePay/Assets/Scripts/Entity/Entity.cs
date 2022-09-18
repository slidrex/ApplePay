using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer SpriteRenderer;
    public Dictionary<byte, byte[]> EffectBundleBuffer = new Dictionary<byte, byte[]>();
    public Dictionary<byte, PayWorld.EffectController.ActiveEffect> ActiveEffects = new Dictionary<byte, PayWorld.EffectController.ActiveEffect>(); 
    public bool Immortal;
    [SerializeField] private GameObject deathParticle, takeDamageParticle, appearParticle;
    [Header("Health")]
    public int MaxHealth = 100;
    [ReadOnly] public int CurrentHealth;
    protected virtual void Awake()
    {
        if(SpriteRenderer == null) SpriteRenderer = GetComponent<SpriteRenderer>();
        PayWorld.Particles.InstantiateParticles(appearParticle, transform.position, Quaternion.identity, 2);
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
        PayWorld.Particles.InstantiateParticles(takeDamageParticle, transform.position, Quaternion.identity, 1.5f, transform);
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
                PayWorld.EffectController.ActiveEffect emptyEffect = new PayWorld.EffectController.ActiveEffect();
                if(ActiveEffects.ElementAt(i).Value.Equals(emptyEffect))
                {
                    ActiveEffects.Remove(ActiveEffects.ElementAt(i).Key);
                    break;
                }
                
                if(ActiveEffects.ElementAt(i).Value.Endless) continue;
                PayWorld.EffectController.ActiveEffect activeEffect = ActiveEffects[ActiveEffects.ElementAt(i).Key];
                activeEffect.RemainTime -= Time.deltaTime;
                for(int j = 0; j < activeEffect.StateEffect.Count; j++)
                {
                    PayWorld.Effect.StateEffect stateEffect = activeEffect.StateEffect[j];
                    PayWorld.EffectController.HandleStateEffect(this, ref stateEffect);
                    activeEffect.StateEffect[j] = stateEffect;
                }
                ActiveEffects[ActiveEffects.ElementAt(i).Key] = activeEffect;
                if(ActiveEffects[ActiveEffects.ElementAt(i).Key].RemainTime <= 0)
                {
                    byte id = ActiveEffects.ElementAt(i).Key;
                    PayWorld.EffectController.RemoveEffect(this, ref id);
                }
        }
        
    }
}