using UnityEngine;

public class CollectableAbility : CollectableItem<CollectableAbility>
{
    [SerializeField] private AbilityHolder.AbilityObject ability;
    public override CollectableAbility CollectableObject => this;
    public Ability Ability { get; set; }
    public KeyCode abilityKeyCode => ability.key;
    protected override string TargetRepository => "ability";
    protected override bool DestroyOnCollect => false;
    protected override void Awake()
    {
        base.Awake();
        Ability = Instantiate(ability.ability);
        Ability.OnInstantiated();
        Ability.OnRepositoryAdded();
    }
}
