namespace PayWorld.Effect
{
    public struct EffectProperty
    {
        public StateEffect StateEffect;
        public string PropertyName;
        public System.Collections.Generic.List<string> Tags { get; private set; }
        public bool IsActive;
        public bool IsNumerable() => StateEffect.Value != null;
        public float GetValue() => StateEffect.GetValue();
        public bool ContainsTag(string tag) => Tags.Contains(tag);
        public void AddTag(string tag) => Tags.Add(tag);
        public void AddTags(string[] tags) => Tags.AddRange(tags);
        public void RemoveTag(string tag)
        {
            if(Tags.Remove(tag) == false) UnityEngine.Debug.LogWarning("Tag \"" + tag + "\" hasn't been found");
        }
        public EffectProperty(StateEffect state, string name, params string[] entryTags)
        {
            StateEffect = state;
            PropertyName = name;
            IsActive = true;
            Tags = new System.Collections.Generic.List<string>();
            AddTags(entryTags);
        }
    }
    public class EffectValue
    {
        public float Value;
        public EffectValue(float value) => Value = value;
    }
    public class StateEffect
    {
        public struct TickImplementation
        {
            public TickImplementation(TickActionHandler action, float tickScale)
            {
                TickAction = action;
                TickScale = tickScale;
                TimeSinceAction = 0;
            }
            public TickActionHandler TickAction;
            public float TickScale {get; private set;}
            public float TimeSinceAction;

        }
        public EffectValue Value;
        public float GetValue() => Value.Value;
        public TickImplementation TickImplement;
        public delegate void TickActionHandler(Entity entity);
        public delegate void BeginActionHandler(Entity entity);
        public delegate void EndActionHandler(Entity entity);
        public BeginActionHandler BeginAction;
        public EndActionHandler EndAction;
        public StateEffect(float targetValue) => Value = new EffectValue(targetValue);
        public StateEffect() {}
        private void Init(BeginActionHandler beginAction, EndActionHandler endAction, TickImplementation tick, EffectValue value)
        {
            BeginAction = beginAction;
            EndAction = endAction;
            TickImplement = tick;
            Value = value;
        }
    }
    public static class States
    {
        private static StateEffect CreateState(float targetValue) => new StateEffect(targetValue);
        private static StateEffect CreateState() => new StateEffect();
        private static StateEffect LinkActions(this StateEffect effect, StateEffect.BeginActionHandler beginAction, StateEffect.EndActionHandler endAction) => effect.LinkActions(beginAction, endAction, new StateEffect.TickImplementation());
        private static StateEffect LinkActions(this StateEffect effect, StateEffect.BeginActionHandler beginAction, StateEffect.EndActionHandler endAction, StateEffect.TickImplementation tick)
        {
            effect.BeginAction = beginAction;
            effect.EndAction = endAction;
            effect.TickImplement = tick;
            return effect;
        }
        public static StateEffect ChangeHealth(float amount, float tickScale)
        {
            StateEffect effect = CreateState(amount);

            StateEffect.TickActionHandler tickAction = (Entity entity) => entity.ChangeHealth((int)(entity.MaxHealth * effect.GetValue()));

            return effect.LinkActions(null, null, new StateEffect.TickImplementation(tickAction, tickScale));
        }
        public static StateEffect ChangeHealth(int amount, float tickScale)
        {
            StateEffect effect = CreateState(amount);

            StateEffect.TickActionHandler tickAction = (Entity entity) => entity.ChangeHealth((int)effect.GetValue());
            
            return effect.LinkActions(null, null, new StateEffect.TickImplementation(tickAction, tickScale));
        }
        public static StateEffect ChangeHealth(int amount)
        {
            StateEffect effect = CreateState(amount);
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.ChangeHealth((int)effect.GetValue());
            return effect.LinkActions(beginAction, null);
        }
        public static StateEffect Strength(float amount)
        {
            StateEffect effect = CreateState(amount);
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<IDamageDealable>().ChangeDamageMultiplier(effect.GetValue());
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<IDamageDealable>().ChangeDamageMultiplier(-effect.GetValue());

            return effect.LinkActions(beginAction, endAction);
        }
        public static StateEffect HoldingHackSpeedChanger(float amount)
        {
            StateEffect stateEffect = CreateState(amount);

            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(stateEffect.GetValue());
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(-stateEffect.GetValue());

            return stateEffect.LinkActions(beginAction, endAction);
        }
        public static StateEffect VelocityChanger(float amount)
        {
            StateEffect stateEffect = CreateState(amount);
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.FindAttribute("movementSpeed").AddMultiplier(amount);
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.FindAttribute("movementSpeed").AddMultiplier(-amount);
            
            return stateEffect.LinkActions(beginAction, endAction);
        }
        public static StateEffect VelocityReverser()
        {
            StateEffect stateEffect = CreateState();
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.FindAttribute("movementSpeed").SetMultiplier(entity.FindAttribute("movementSpeed").GetMultiplier() * -1);
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.FindAttribute("movementSpeed").SetMultiplier(entity.FindAttribute("movementSpeed").GetMultiplier() * -1);
            
            return stateEffect.LinkActions(beginAction, endAction);
        }
        
        
        public static StateEffect MoveConstraint()
        {
            StateEffect stateEffect = CreateState();
            StateEffect.BeginActionHandler action = (Entity entity) => entity.GetComponent<EntityMovement>().MoveDisable = true;
            
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<EntityMovement>().MoveDisable = false;
            
            return stateEffect.LinkActions(action, endAction);
        }
        public static StateEffect WeaponConstraint()
        {
            StateEffect stateEffect = CreateState();
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = true;
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = false;

            return stateEffect.LinkActions(beginAction, endAction);
        }
    }
}