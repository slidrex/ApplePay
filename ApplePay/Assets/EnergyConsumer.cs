using UnityEngine;

public class EnergyConsumer : MonoBehaviour
{
    [field: SerializeField] public float EnergySubtractMultiplier { get; protected set; } = 1.0f;
    [field: SerializeField] public float EnergyAddMultiplier { get; protected set; } = 1.0f;
    [field: SerializeField] public int MaxEnergy { get; protected set; }
    public int CurrentEnergy { get; protected set; }
    private Creature entity;
    public EnergyBar energyBar;
    public void SetOwner(Creature entity) => this.entity = entity;
    protected virtual void Start()
    {
        SetupAttributes();
        CurrentEnergy = MaxEnergy;
        energyBar?.IndicatorSetup(entity);
    }
    private void SetupAttributes()
    {
        entity.AddAttribute("maxEnergy", new FloatRef( () => MaxEnergy,
            v => MaxEnergy = (int)v
        ), MaxEnergy);
        entity.AddAttribute("energyAddMultiplier", new FloatRef( () => EnergyAddMultiplier,
            val => EnergyAddMultiplier = val
        ), EnergyAddMultiplier);
        entity.AddAttribute("energyConsumeMultiplier", new FloatRef( () => EnergySubtractMultiplier,
            val => EnergySubtractMultiplier = val
        ), EnergySubtractMultiplier);
    }
    public bool TryConsumeEnergy(int amount)
    {
        if(CanConsume(amount)) 
        {
            SubtractEnergy(amount);
            return true;
        }
        return false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddEnergy(10);
        }
    }
    public bool CanConsume(int amount) => CurrentEnergy - amount >= 0;
    protected virtual void SubtractEnergy(int amount)
    {
        CurrentEnergy -= (int)(amount * EnergySubtractMultiplier);
        energyBar?.IndicatorUpdate();
    }
    public virtual void AddEnergy(int amount)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + (int)(amount * EnergyAddMultiplier), 0, MaxEnergy);
        energyBar?.IndicatorUpdate();
    }
}
