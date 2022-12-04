namespace PayWorld.Effect
{
    public struct EffectProperty
    {
        public EffectAction EffectAction;
        public EffectProperty(EffectAction state) => EffectAction = state;
    }
    public class EffectAction
    {
        public struct TickImplementation
        {
            public TickImplementation(EffectActionHandler action, float tickScale)
            {
                TickAction = action;
                TickScale = tickScale;
                TimeSinceAction = 0;
            }
            public EffectActionHandler TickAction;
            public float TickScale {get; private set;}
            public float TimeSinceAction;

        }
        public VirtualBase Value;
        public float GetValue() => Value.GetValue();
        public TickImplementation TickImplement;
        public delegate void EffectActionHandler(Entity entity);
        public EffectActionHandler BeginAction;
        public EffectActionHandler EndAction;
        public EffectAction(float targetValue) => Value = new VirtualBase(targetValue);
        public EffectAction() {}
        private void Init(EffectActionHandler beginAction, EffectActionHandler endAction, TickImplementation tick, VirtualBase value)
        {
            BeginAction = beginAction;
            EndAction = endAction;
            TickImplement = tick;
            Value = value;
        }
    }
    public static class EffectActionPresets
    {
        private static EffectAction CreateState(float targetValue) => new EffectAction(targetValue);
        private static EffectAction CreateEmptyState() => new EffectAction();
        private static EffectAction LinkActions(this EffectAction effect, EffectAction.EffectActionHandler beginAction, EffectAction.EffectActionHandler endAction) => effect.LinkActions(beginAction, endAction, new EffectAction.TickImplementation());
        private static EffectAction LinkActions(this EffectAction effect, EffectAction.EffectActionHandler beginAction, EffectAction.EffectActionHandler endAction, EffectAction.TickImplementation tick)
        {
            effect.BeginAction = beginAction;
            effect.EndAction = endAction;
            effect.TickImplement = tick;
            return effect;
        }
        public static EffectAction ChangeHealth(float amount, float tickScale)
        {
            EffectAction effect = CreateState(amount);

            EffectAction.EffectActionHandler tickAction = (Entity entity) => entity.ChangeHealth((int)(entity.MaxHealth * effect.GetValue()));

            return effect.LinkActions(null, null, new EffectAction.TickImplementation(tickAction, tickScale));
        }
        public static EffectAction ChangeHealth(int amount, float tickScale)
        {
            EffectAction effect = CreateState(amount);

            EffectAction.EffectActionHandler tickAction = (Entity entity) => entity.ChangeHealth((int)effect.GetValue());
            
            return effect.LinkActions(null, null, new EffectAction.TickImplementation(tickAction, tickScale));
        }
        public static EffectAction ChangeHealth(int amount)
        {
            EffectAction effect = CreateState(amount);
            EffectAction.EffectActionHandler beginAction = (Entity entity) => entity.ChangeHealth((int)effect.GetValue());
            return effect.LinkActions(beginAction, null);
        }
        public static EffectAction AttributeMultiply(string attributeName, float value)
        {
            EffectAction EffectAction = CreateState(value);
            EntityAttribute.TagAttribute tag = null;
            EffectAction.EffectActionHandler beginAction = (Entity entity) => tag = entity.FindAttribute(attributeName).AddAttributeMultiplier(value, "effect-multiplier");
            EffectAction.EffectActionHandler endAction = (Entity entity) => tag?.Remove();

            return EffectAction.LinkActions(beginAction, endAction);
        }
        public static EffectAction AttributePercent(string attributeName, float value)
        {
            EffectAction EffectAction = CreateState(value);
            EntityAttribute.TagAttribute tag = null;
            EffectAction.EffectActionHandler beginAction = (Entity entity) => tag = entity.FindAttribute(attributeName).AddPercent(new VirtualBase.VirtualFloatRef(() => EffectAction.Value.GetValue()) , "effect-multiplier");
            EffectAction.EffectActionHandler endAction = (Entity entity) => tag?.Remove();

            return EffectAction.LinkActions(beginAction, endAction);
        }
        public static EffectAction MoveConstraint()
        {
            EffectAction EffectAction = CreateEmptyState();
            EntityAttribute.TagAttribute tagAttribute = null;
            EffectAction.EffectActionHandler action = delegate(Entity entity) 
            {
                EntityAttribute attribute = entity.FindAttribute("movementSpeed");
                tagAttribute = attribute.AddAttributeMultiplier(0);
            };
            
            EffectAction.EffectActionHandler endAction = (Entity entity) => tagAttribute.Remove();
            
            return EffectAction.LinkActions(action, endAction);
        }
        public static EffectAction WeaponConstraint()
        {
            EffectAction EffectAction = CreateEmptyState();
            EffectAction.EffectActionHandler beginAction = (Entity entity) => entity.GetComponent<WeaponHolder>()?.Disables.Add(0);
            EffectAction.EffectActionHandler endAction = (Entity entity) => entity.GetComponent<WeaponHolder>()?.Disables.Remove(0);

            return EffectAction.LinkActions(beginAction, endAction);
        }
    }
}