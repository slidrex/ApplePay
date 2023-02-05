using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private Creature entity;
    private AbilityRepository abilityRepository;
    private EnergyConsumer consumer;
    private void Awake()
    {
        abilityRepository = (AbilityRepository)entity.InventorySystem.GetRepository("ability");
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
        for(int i = 0; i < abilityRepository.Items.Length; i++)
        {
            CollectableAbility ability = abilityRepository.Items[i];
            if(ability == null) continue;
            if(Input.GetKeyDown(ability.abilityKeyCode) && isFree)
            {
                TryExecuteAbility(ability.Ability);
            }
            ability.Ability.OnUpdate();

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
