using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyConsumer : MonoBehaviour
{
    public float EnergySubtractMultiplier { get; private set; }
    public float EnergyAddMultiplier { get; private set; }
    public float EnergyRegenerationRate { get; private set; }
    public float MaxEnergy { get; private set; }
    public float CurrentEnergy { get; private set; }
    public void addattribs()
    {
        //this.AddAttribute("attackDamage", new FloatRef(
        //    () => AttackDamage,
        //    val => AttackDamage = (int)val
        //), AttackDamage);
    }
    public bool TryConsumeEnergy(float amount)
    {
        if(CanConsume(amount)) 
        {
            SubtractEnergy(amount);
            return true;
        }
        return false;
    }
    public bool CanConsume(float amount) => CurrentEnergy - amount >= 0;
    protected virtual void SubtractEnergy(float amount)
    {
        CurrentEnergy -= amount * EnergySubtractMultiplier;
    }
    public virtual void AddEnergy(int amount)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount * EnergyAddMultiplier, 0, MaxEnergy);
    }
}
