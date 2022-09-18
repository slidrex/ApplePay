using System.Linq;
using UnityEngine;
using PayWorld.Effect;

namespace PayWorld
{
    public class EffectController : MonoBehaviour
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
            
            ActiveEffect effect = CreateEffect(duration, false, databaseEffect.Properties);
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
        private static ActiveEffect CreateEffect(float duration, bool endless, EffectProperty[] properties) => new ActiveEffect(properties.ToList(), duration, endless);
        ///<summary>
        ///Attaches display attribute to an active effect.
        ///</summary>
        public static void AttachVisualAttrib(ActiveEffect effect, string name, string description, string index, Sprite sprite, Pay.UI.UIManager.TextField[] additionals)
        {
            VisualAttribHandler(effect, name, description, index, sprite, additionals);
        }
        public static void AddTags(ActiveEffect effect, params string[] tags) 
        {
            foreach(EffectProperty property in effect.EffectProperties)
            {
                property.AddTags(tags);
            }
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
            internal float RemainTime;
            internal bool Endless;
            public EffectDisplay EffectDisplay;
            public ActiveEffect(System.Collections.Generic.List<EffectProperty> states, float duration, bool endless)
            {
                EffectProperties = states;
                RemainTime = duration;
                Endless = endless;
            }
            public ActiveEffect() { }
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
