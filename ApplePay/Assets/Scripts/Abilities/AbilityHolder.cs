using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private Creature entity;
    [SerializeField] private System.Collections.Generic.List<AbilityObject> Abilities;
    private EnergyConsumer consumer;
    private void Awake()
    {
        for(int i = 0; i < Abilities.Count; i++)
        {
            Abilities[i] = new AbilityObject() { ability = Instantiate(Abilities[i].ability), key = Abilities[i].key};
            Abilities[i].ability.OnInstantiated();
        }
        consumer = (entity as IEnergyConsumer).Consumer;
    }
    private void Update()
    {
        InputAbilities();
    }
    protected void OnAbilityBegin(Ability ability) {}
    protected void OnAbilityEnd(Ability ability) {}
    private void InputAbilities()
    {
        bool isFree = entity.IsFree();
        foreach(AbilityObject abilityObject in Abilities)
        {
            if(Input.GetKeyDown(abilityObject.key) && isFree)
            {
                TryExecuteAbility(abilityObject.ability);
            }
            abilityObject.ability.OnUpdate();
        }
    }
    private void TryExecuteAbility(Ability ability)
    {
        if(ability.IsActivatable() && consumer.TryConsumeEnergy(ability.GetEnergyCost()))
        {
            ability.BeginAbility(entity, OnAbilityEnd);
            OnAbilityBegin(ability);
        }
    }
    [System.Serializable]
    public struct AbilityObject
    {
        public Ability ability;
        public KeyCode key; 
    }
}
