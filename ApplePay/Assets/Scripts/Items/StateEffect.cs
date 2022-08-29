namespace PayWorld.Effect
{
public class StateEffect
{
    public delegate void BeginActionHandler(Entity entity);
    public delegate void EndActionHandler(Entity entity);
    public BeginActionHandler BeginAction;
    public EndActionHandler EndAction;
    public StateEffect(BeginActionHandler beginAction, EndActionHandler endAction) 
    {
        BeginAction = beginAction;
        EndAction = endAction;
    }
}
public class States
{
    public static StateEffect Strength(float changeAmount)
    {
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<IDamageDealable>().ChangeDamageMultiplier(changeAmount);
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<IDamageDealable>().ChangeDamageMultiplier(-changeAmount);

        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
    public static StateEffect HoldingHackSpeedChanger(float changeAmount)
    {
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(changeAmount);
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(-changeAmount);

        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
    public static StateEffect VelocityChanger(float amount)
    {
        float changedValue = 0;
        
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<EntityMovement>().ChangeSpeedMultiplier(amount, out changedValue);
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<EntityMovement>().ChangeSpeedMultiplier(-changedValue, out float changed);
        
        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
    public static StateEffect VelocityReverser()
    {
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<EntityMovement>().ReverseControl();
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<EntityMovement>().ReverseControl();
        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
    public static StateEffect MoveConstraint()
    {
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<EntityMovement>().MoveDisable = true;
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<EntityMovement>().MoveDisable = false;
        

        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
    public static StateEffect WeaponConstraint()
    {
        StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = true;
        StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = false;
        

        StateEffect stateEffect = new StateEffect(beginAction, endAction);
        return stateEffect;
    }
}
}