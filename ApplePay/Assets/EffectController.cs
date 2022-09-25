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
        public static void CreateBundle(Entity applyEntity, out byte id, params byte[] bundledId)
        {
            byte[] usedID = Pay.Functions.Generic.CombineArrays(applyEntity.ActiveEffects.Keys.ToArray(), applyEntity.EffectBundleBuffer.Keys.ToArray());
            byte _id = Pay.Functions.Math.GetUniqueByte(usedID, 0);
            applyEntity.EffectBundleBuffer.Add(_id, bundledId);
            id = _id;
        }
        ///<summary>
        ///Adds effect to the specified entity by template.
        ///</summary>
        public static ActiveEffect AddEffect(Entity applyEntity, string id, byte level, float duration)
        {
            EffectDatabase effectDatabase = new EffectDatabase(level);
            if(effectDatabase.Effects.ContainsKey(id) == false) 
            {
                Debug.LogWarning("Invalid effect id (" + id + ").");
                return null;
            }
            effectDatabase.Effects.TryGetValue(id, out EffectTemplate databaseEffect);
            
            ActiveEffect effect = CreateEffect(duration, false, databaseEffect.Properties, databaseEffect.EntryTag, "activeEffect");
            AttachVisualAttrib(effect, databaseEffect.TemplateDisplay.Name, databaseEffect.TemplateDisplay.Description, databaseEffect.TemplateDisplay.Index, databaseEffect.TemplateDisplay.Sprite, databaseEffect.TemplateDisplay.Additionals);
            return AddEffect(applyEntity, effect, out byte _id);
        }
        ///<summary>
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static ActiveEffect AddEffect(Entity applyEntity, float duration, out byte id, params EffectProperty[] stateEffects) => AddEffect(applyEntity, duration, false, out id, stateEffects);
        ///<summary>
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static ActiveEffect AddEffect(Entity applyEntity, out byte id, params EffectProperty[] stateEffects) => AddEffect(applyEntity, 0f, true, out id, stateEffects);
        private static ActiveEffect AddEffect(Entity applyEntity, float duration, bool endless, out byte id, EffectProperty[] stateEffects)
        {
            ActiveEffect activeEffect = CreateEffect(duration, endless, stateEffects);
            AddEffect(applyEntity, activeEffect, out id);
            return activeEffect;
        }
        private static ActiveEffect AddEffect(Entity entity, ActiveEffect effect, out byte id)
        {

            byte[] usedID = Pay.Functions.Generic.CombineArrays(entity.ActiveEffects.Keys.ToArray(), entity.EffectBundleBuffer.Keys.ToArray());
            byte _id = Pay.Functions.Math.GetUniqueByte(usedID, 0);
            StateEffect[] stateEffects = effect.EffectProperties.Select(x => x.StateEffect).ToArray();
            foreach(StateEffect stateEffect in stateEffects)
                stateEffect.BeginAction?.Invoke(entity);
            
            entity.ActiveEffects.Add(_id, effect);
            
            entity.GetComponent<IEffectUpdateHandler>()?.OnEffectUpdated();
            id = _id;
            return effect;
        }
        private static ActiveEffect CreateEffect(float duration, bool endless, EffectProperty[] properties, params string[] tags) => new ActiveEffect(properties.ToList(), duration, endless, tags);
        ///<summary>
        ///Attaches display attribute to an active effect.
        ///</summary>
        public static void AttachVisualAttrib(ActiveEffect effect, string name, string description, string index, Sprite sprite, Pay.UI.UIManager.TextField[] additionals)
        {
            VisualAttribHandler(effect, name, description, index, sprite, additionals);
        }
        ///<summary>
        ///Attaches display attribute to an active effect.
        ///</summary>
        public static void AttachVisualAttrib(ActiveEffect effect, string name, string description, string index, Sprite sprite)
        {
            VisualAttribHandler(effect, name, description, index, sprite, null);
        }
        private static void VisualAttribHandler(ActiveEffect effect, string name, string description, string index, Sprite sprite, Pay.UI.UIManager.TextField[] additionals)
        {
            EffectController.EffectDisplay effectDisplay = new EffectDisplay(sprite, name, description, index, additionals);
            
            effect.EffectDisplay = effectDisplay;
        }
    
        ///<summary>
        ///Removes effect (or all the effects if it is a bundle) from the specified entity by index.
        ///</summary>
        public static void RemoveEffect(Entity applyEntity, ref byte id)
        {
            bool isBundle = applyEntity.EffectBundleBuffer.ContainsKey(id);
            if(isBundle == false)
            {
                RemoveSingle(applyEntity,ref id);
            }
            else
            {
                RemoveBundle(applyEntity, ref id);
            }
            if(applyEntity.GetComponent<IEffectUpdateHandler>() != null)
                applyEntity.GetComponent<IEffectUpdateHandler>().OnEffectUpdated();
        }
        private static void RemoveSingle(Entity entity, ref byte effectId)
        {
            entity.ActiveEffects.TryGetValue(effectId, out ActiveEffect activeEffect);
            StateEffect[] stateEffects = activeEffect.EffectProperties.Select(x => x.StateEffect).ToArray();
            foreach(StateEffect state in stateEffects)
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
                StateEffect[] stateEffects = activeEffect.EffectProperties.Select(x => x.StateEffect).ToArray();
                foreach(StateEffect state in stateEffects)
                    state.EndAction.Invoke(entity);
                entity.ActiveEffects.Remove(id);
            }
            entity.EffectBundleBuffer.Remove(bundleId);

        }
        public class ActiveEffect
        {
            internal System.Collections.Generic.List<EffectProperty> EffectProperties = new System.Collections.Generic.List<EffectProperty>();
            public System.Collections.Generic.List<string> Tags = new System.Collections.Generic.List<string>();
            public System.Collections.Generic.List<EffectMask> Masks = new System.Collections.Generic.List<EffectMask>();
            public float RemainTime;
            internal float baseRemainTime;
            internal bool Endless;
            public EffectDisplay EffectDisplay;
            public ActiveEffect(System.Collections.Generic.List<EffectProperty> states, float duration, bool endless, params string[] tags)
            {
                EffectProperties = states;
                baseRemainTime = duration;
                RemainTime = duration;
                Endless = endless;
                Tags.AddRange(tags);
            }
            public ActiveEffect() { }
        }
        public static EffectMask AddMask(this ActiveEffect effect, EffectMask.MaskedParameter parameter, AttributeOperation operation, float value)
        {
            EffectMask mask = new EffectMask(parameter, operation, value, effect);
            effect.Masks.Add(mask);
            effect.UpdateEffectMasks();
            return mask;
        }
        public static void Remove(this EffectMask mask)
        {
            mask.AttachedEffect.Masks.Remove(mask);
            mask.AttachedEffect.UpdateEffectMasks();
        }
        private static void UpdateEffectMasks(this ActiveEffect effect)
        {
            effect.RemainTime = effect.baseRemainTime;
            foreach(EffectProperty property in effect.EffectProperties)
            {
                if(property.StateEffect.Value != null)
                    property.StateEffect.Value.Value = property.StateEffect.Value.BaseValue;
            }
            foreach(EffectMask mask in effect.Masks) effect += mask;
        }
        public struct EffectMask
        {
            public ActiveEffect AttachedEffect;
            public float Value;
            public AttributeOperation Operation;
            public MaskedParameter Parameter;
            public enum MaskedParameter
            {
                RemainTime,
                EffectValue
            }
            public static ActiveEffect operator +(ActiveEffect effect, EffectMask mask)
            {
                UnityEngine.Vector2 time = new Vector2();
                UnityEngine.Vector2 value = new Vector2();

                if(mask.Parameter == EffectMask.MaskedParameter.RemainTime)
                    time += mask.Operation == AttributeOperation.Add ? mask.Value * Vector2.right : Vector2.up *  mask.Value;
                
                else if(mask.Parameter == EffectMask.MaskedParameter.EffectValue)
                    value += mask.Operation == AttributeOperation.Add ? mask.Value * Vector2.right : Vector2.up * mask.Value;
                
                effect.RemainTime = (effect.RemainTime + time.x) * time.y;
                
                foreach(EffectProperty property in effect.EffectProperties)
                {
                    if(property.StateEffect.Value != null)
                        property.StateEffect.Value.Value = (property.StateEffect.Value.Value + value.x) * value.y;
                }
                
                return effect;
            }
            public EffectMask(MaskedParameter parameter, AttributeOperation operation, float value, ActiveEffect attachedEffect)
            {
                Value = value;
                Operation = operation;
                Parameter = parameter;
                AttachedEffect = attachedEffect;
            }
        }
        public struct EffectDisplay
        {
            public EffectDisplay(Sprite sprite, string name, string description, string index, params Pay.UI.UIManager.TextField[] additionals)
            {
                Sprite = sprite;
                Name = name;
                Description = description;
                Index = index;
                Additionals = additionals;
            }
            public Sprite Sprite;
            public string Name;
            public string Description;
            public string Index;
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