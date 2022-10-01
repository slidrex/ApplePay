namespace PayWorld.Effect
{
    public struct EffectProperty
    {
        public StateEffect StateEffect;
        public EffectProperty(StateEffect state) => StateEffect = state;
    }
    public class EffectValue
    {
        public float Value;
        public float BaseValue;
        public EffectValue(float value)
        {
            BaseValue = value;
            Value = value;
        }
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
            StateEffect.BeginActionHandler beginAction = (Entity entity)  => entity.FindAttribute("attack_damage")?.AddMultiplier(effect.GetValue());
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.FindAttribute("attack_damage")?.AddMultiplier(-effect.GetValue());

            return effect.LinkActions(beginAction, endAction);
        }
        public static StateEffect HoldingHackSpeedChanger(float amount)
        {
            StateEffect stateEffect = CreateState(amount);
            TagAttribute tag = null;
            
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(stateEffect.GetValue());
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<InteractManager>().ChangeHackSpeed(-stateEffect.GetValue());

            return stateEffect.LinkActions(beginAction, endAction);
        }
        public static StateEffect VelocityChanger(float amount)
        {
            StateEffect stateEffect = CreateState(amount);
            TagAttribute tag = null;
            StateEffect.BeginActionHandler beginAction = (Entity entity) => tag = entity.FindAttribute("movementSpeed").AddTaggedAttribute(stateEffect.GetValue(), AttributeType.Multiplier);
            StateEffect.EndActionHandler endAction = (Entity entity) => tag.Remove();
            
            return stateEffect.LinkActions(beginAction, endAction);
        }
        public static StateEffect VelocityReverser()
        {
            StateEffect stateEffect = CreateState();
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.FindAttribute("movementSpeed").SetMultiplier(entity.FindAttribute("movementSpeed").ValueMultiplier * -1);
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.FindAttribute("movementSpeed").SetMultiplier(entity.FindAttribute("movementSpeed").ValueMultiplier * -1);
            
            return stateEffect.LinkActions(beginAction, endAction);
        }
        public static StateEffect MoveConstraint()
        {
            StateEffect stateEffect = CreateState();
            
            StateEffect.BeginActionHandler action = delegate(Entity entity) 
            {
                EntityAttribute attribute = entity.FindAttribute("movementSpeed");
                if(attribute.ContainsTaggedAttribute("effectMovementConstrinaint") == false)
                    attribute.SetTaggedAttribute(0f, AttributeType.Multiplier, "effectMovementConstrinaint");
                
                attribute.GetTagAttributes("effectMovementConstrinaint")[0].Count ++;
            };
            
            StateEffect.EndActionHandler endAction = delegate(Entity entity) 
            {
                EntityAttribute attribute = entity.FindAttribute("movementSpeed");
                attribute.GetTagAttributes("effectMovementConstrinaint")[0].Count--;
                if(attribute.GetTagAttributes("effectMovementConstrinaint")[0].Count == 0)
                    attribute.GetTagAttributes("effectMovementConstrinaint")[0].Remove();
            };
            
            return stateEffect.LinkActions(action, endAction);
        }
        /*public static StateEffect WeaponConstraint()
        {
            StateEffect stateEffect = CreateState();
            StateEffect.BeginActionHandler beginAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = true;
            StateEffect.EndActionHandler endAction = (Entity entity) => entity.GetComponent<WeaponHolder>().Disable = false;

            return stateEffect.LinkActions(beginAction, endAction);
        }
        */
    }
}