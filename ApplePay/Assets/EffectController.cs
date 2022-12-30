using System.Linq;
using UnityEngine;
using PayWorld.Effect;

namespace PayWorld
{
    public static class EffectController
    {
        ///<summary>
        ///Generates index that can be used for access all the bundled effects.
        ///</summary>
        public static void CreateBundle(Entity entity, out byte id, params byte[] bundledId)
        {
            byte[] usedID = Pay.Functions.Generic.CombineArrays(entity.ActiveEffects.Keys.ToArray(), entity.EffectBundleBuffer.Keys.ToArray());
            byte _id = Pay.Functions.Math.GetUniqueByte(usedID, 0);
            entity.EffectBundleBuffer.Add(_id, bundledId);
            id = _id;
        }
        ///<summary>
        ///Adds effect to the specified entity by template.
        ///</summary>
        public static ActiveEffect AddEffect(Entity entity, string id, byte level, float duration)
        {
            EffectDatabase effectDatabase = new EffectDatabase(level);
            if(effectDatabase.Effects.ContainsKey(id) == false) 
            {
                Debug.LogWarning("Invalid effect id (" + id + ").");
                return null;
            }
            effectDatabase.Effects.TryGetValue(id, out EffectTemplate databaseEffect);
            
            ActiveEffect effect = CreateEffect(entity, duration, false, databaseEffect.Properties, databaseEffect.EntryTag, "activeEffect");
            effect.AttachVisualAttrib(databaseEffect.TemplateDisplay.Name, databaseEffect.TemplateDisplay.Description, databaseEffect.TemplateDisplay.Index, databaseEffect.TemplateDisplay.Sprite, databaseEffect.TemplateDisplay.Additionals);
            return AddEffect(entity, effect, out byte _id);
        }
        ///<summary>
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static ActiveEffect AddEffect(Entity entity, float duration, out byte id, params EffectProperty[] EffectActions) => AddEffect(entity, duration, false, out id, EffectActions);
        ///<summary>
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static ActiveEffect AddEffect(Entity entity, out byte id, params EffectProperty[] EffectActions) => AddEffect(entity, 0f, true, out id, EffectActions);
        private static ActiveEffect AddEffect(Entity entity, float duration, bool endless, out byte id, EffectProperty[] EffectActions)
        {
            ActiveEffect activeEffect = CreateEffect(entity, duration, endless, EffectActions);
            AddEffect(entity, activeEffect, out id);
            return activeEffect;
        }
        private static ActiveEffect AddEffect(Entity entity, ActiveEffect effect, out byte id)
        {

            byte[] usedID = Pay.Functions.Generic.CombineArrays(entity.ActiveEffects.Keys.ToArray(), entity.EffectBundleBuffer.Keys.ToArray());
            byte _id = Pay.Functions.Math.GetUniqueByte(usedID, 0);
            EffectAction[] EffectActions = effect.EffectProperties.Select(x => x.EffectAction).ToArray();
            foreach(EffectAction EffectAction in EffectActions)
                EffectAction.BeginAction?.Invoke(entity);
            
            entity.ActiveEffects.Add(_id, effect);
            
            entity.GetComponent<IEffectUpdateHandler>()?.OnEffectUpdated();
            id = _id;
            return effect;
        }
        private static ActiveEffect CreateEffect(Entity entity, float duration, bool endless, EffectProperty[] properties, params string[] tags) => new ActiveEffect(entity, properties, duration, endless, tags);
        ///<summary>
        ///Attaches display attribute to an active effect.
        ///</summary>
        public static ActiveEffect AttachVisualAttrib(this ActiveEffect effect, string name, string description, string index, Sprite sprite, params Pay.UI.UIManager.TextField[] additionals)
        {
            EffectController.EffectDisplay effectDisplay = new EffectDisplay(sprite, name, description, effect.EffectProperties.Select(x => x.EffectAction).ToArray(), index, additionals);
            
            effect.EffectDisplay = effectDisplay;
            return effect;
        }
    
        ///<summary>
        ///Removes effect (or all the effects if it is a bundle) from the specified entity by index.
        ///</summary>
        public static void RemoveEffect(Entity entity, ref byte id)
        {
            bool isBundle = entity.EffectBundleBuffer.ContainsKey(id);
            if(isBundle == false)
            {
                RemoveSingle(entity,ref id);
            }
            else
            {
                RemoveBundle(entity, ref id);
            }
            if(entity.GetComponent<IEffectUpdateHandler>() != null)
                entity.GetComponent<IEffectUpdateHandler>().OnEffectUpdated();
        }
        private static void RemoveSingle(Entity entity, ref byte effectId)
        {
            entity.ActiveEffects.TryGetValue(effectId, out ActiveEffect activeEffect);
            EffectAction[] EffectActions = activeEffect.EffectProperties.Select(x => x.EffectAction).ToArray();
            foreach(EffectAction state in EffectActions)
            {
                state.EndAction?.Invoke(entity);
            }
            entity.ActiveEffects.Remove(effectId);
            effectId = 0;
        }
        private static void RemoveBundle(Entity entity, ref byte bundleId)
        {
            entity.EffectBundleBuffer.TryGetValue(bundleId, out byte[] bundledId);
            foreach(byte id in bundledId)
            {
                entity.ActiveEffects.TryGetValue(id, out ActiveEffect activeEffect);
                EffectAction[] EffectActions = activeEffect.EffectProperties.Select(x => x.EffectAction).ToArray();
                foreach(EffectAction state in EffectActions)
                    state.EndAction.Invoke(entity);
                entity.ActiveEffects.Remove(id);
            }
            entity.EffectBundleBuffer.Remove(bundleId);

        }
        [System.Serializable]
        public class ActiveEffect
        {
            public Entity Owner;
            internal EffectProperty[] EffectProperties;
            public System.Collections.Generic.List<string> Tags = new System.Collections.Generic.List<string>();            
            private VirtualBase remainTime;
            public float RemainTimeSourceValue 
            {
                get => remainTime.SourceValue;
                set => remainTime.SourceValue = value;
            }
            public float ResultRemainTime => remainTime.GetValue();
            internal bool Endless;
            public EffectDisplay EffectDisplay;
            public ActiveEffect(Entity entity, EffectProperty[] states, float duration, bool endless, params string[] tags)
            {
                EffectProperties = states;
                remainTime = new VirtualBase(duration);
                Endless = endless;
                Tags.AddRange(tags);
                Owner = entity;
            }
            public Modifier AddRemainTimeModifier(VirtualBase.VirtualFloatRef multiplier, params string[] tags) => new Modifier(default(VirtualBase.BaseValue[]), remainTime.AddMultiplier(multiplier, tags), Owner);
            public Modifier AddValueModifier(VirtualBase.VirtualFloatRef multiplier, params string[] tags)
            {
                VirtualBase.BaseValue[] baseValueArray = new VirtualBase.BaseValue[EffectProperties.Length];
                for(int i = 0; i < EffectProperties.Length; i++)
                {
                    baseValueArray[i] = EffectProperties[i].EffectAction.Value.AddMultiplier(multiplier, tags);
                    
                    foreach(EntityAttribute attribute in Owner.Attributes.Values)
                    {
                        attribute.ApplyResult();
                    }
                }
                Modifier modifier = new Modifier(baseValueArray, default(VirtualBase.BaseValue), Owner);
                return modifier;
            }
            public struct Modifier
            {
                private VirtualBase.BaseValue[] valueModifiers;
                private VirtualBase.BaseValue timeModifier;
                private Entity owner;
                internal Modifier(VirtualBase.BaseValue[] valueMod, VirtualBase.BaseValue timeMod, Entity entity)
                {
                    valueModifiers = valueMod;
                    timeModifier = timeMod;
                    owner = entity;
                }
                public void Remove()
                {
                    foreach(VirtualBase.BaseValue baseValue in valueModifiers)
                    {
                        baseValue.Remove();
                    }
                    timeModifier.Remove();
                    foreach(EntityAttribute attribute in owner.Attributes.Values)
                    {
                        attribute.ApplyResult();
                    }
                }
            }
            public ActiveEffect() { }
        }
        public struct EffectDisplay
        {
            public string FormatDescription(string description)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("{(\\d+),(.*?)}");
                System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(description);
                
                foreach(System.Text.RegularExpressions.Match match in matchCollection)
                {
                    int startIndex = description.IndexOf(match.Value);
                    
                    description = description.Remove(startIndex, match.Value.Length);
                    
                    description = description.Insert(startIndex, (EffectFormatValues[int.Parse(match.Groups[1].Value)].GetValue() * float.Parse(match.Groups[2].Value)).ToString());
                    
                }
                return description;
            }
            public EffectDisplay(Sprite sprite, string name, string description, EffectAction[] descriptionFormatValues, string index, params Pay.UI.UIManager.TextField[] additionals)
            {
                Sprite = sprite;
                Name = name;
                EffectFormatValues = descriptionFormatValues;
                Description = description;
                Index = index;
                Additionals = additionals;
            }
            public Sprite Sprite;
            public string Name;
            public string Description;
            public string Index;
            public EffectAction[] EffectFormatValues;
            public Pay.UI.UIManager.TextField[] Additionals;
        }
        public static EffectTemplate GetEffectTemplate(string effectID, byte level)
        {
            EffectDatabase effectDatabase = new EffectDatabase(level);
            effectDatabase.Effects.TryGetValue(effectID, out EffectTemplate databaseEffect);
            return databaseEffect;
        }
    }
}