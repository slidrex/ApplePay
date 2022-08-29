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
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static void AddEffect(Entity applyEntity, float duration, out byte id, params StateEffect[] stateEffects)
        {
            AddEffect(applyEntity, duration, false, out id, stateEffects);
        }
        ///<summary>
        ///Adds effect to selected entity (use "AttachVisualAttrib" to specify effect visual attributes).
        ///</summary>
        public static void AddEffect(Entity applyEntity, out byte id, params StateEffect[] stateEffects)
        {
            AddEffect(applyEntity, 0f, true, out id, stateEffects);
        }
        private static void AddEffect(Entity applyEntity, float duration, bool endless, out byte id, StateEffect[] stateEffects)
        {
            byte[] usedID = Pay.Functions.Generic.CombineArrays(applyEntity.ActiveEffects.Keys.ToArray(), applyEntity.EffectBundleBuffer.Keys.ToArray());
            byte _id = Pay.Functions.Math.GetUniqueByte(usedID, 0);
            ActiveEffect activeEffect = new ActiveEffect();
            foreach(StateEffect stateEffect in stateEffects)
                stateEffect.BeginAction.Invoke(applyEntity);
            activeEffect = InstantiateActiveEffect(stateEffects.ToList(), duration, endless);
            applyEntity.ActiveEffects.Add(_id, activeEffect);
            if(applyEntity.GetComponent<IEffectUpdateHandler>() != null)
                applyEntity.GetComponent<IEffectUpdateHandler>().OnEffectUpdated();
            id = _id;
        }
        
        ///<summary>
        ///Attaches display attribute to an active effect.
        ///</summary>
        public static void AttachVisualAttrib(Entity wrappedEntity, byte effectID, string name, string description, string index, Sprite sprite, params string[] additionals)
        {
            VisualAttribHandler(wrappedEntity, effectID, name, description, index, sprite, additionals);
        }
        private static void VisualAttribHandler(Entity wrappedEntity, byte effectID, string name, string description, string index, Sprite sprite, params string[] additionals)
        {
            wrappedEntity.ActiveEffects.TryGetValue(effectID, out PayWorld.EffectController.ActiveEffect activeEffect);
            
            EffectController.EffectDisplay effectDisplay = new EffectDisplay();
            effectDisplay.Description = description;
            effectDisplay.Name = name;
            effectDisplay.Sprite = sprite;
            effectDisplay.Additionals = additionals;
            effectDisplay.Index = index;
            activeEffect.EffectDisplay = effectDisplay;
            
            wrappedEntity.ActiveEffects[effectID] = activeEffect;
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
            foreach(StateEffect state in activeEffect.StateEffect)
            {
                state.EndAction.Invoke(entity);
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
                foreach(StateEffect state in activeEffect.StateEffect)
                    state.EndAction.Invoke(entity);
                entity.ActiveEffects.Remove(id);
            }
            entity.EffectBundleBuffer.Remove(bundleId);

        }
        private static ActiveEffect InstantiateActiveEffect(System.Collections.Generic.List<StateEffect> states, float duration, bool endless)
        {
            ActiveEffect activeEffect = new ActiveEffect();
            activeEffect.StateEffect = states;
            activeEffect.RemainTime = duration;
            activeEffect.Endless = endless;
            return activeEffect;
        }
        public class ActiveEffect
        {
            internal System.Collections.Generic.List<StateEffect> StateEffect = new System.Collections.Generic.List<StateEffect>();
            internal float RemainTime;
            internal bool Endless;
            public EffectDisplay EffectDisplay;
        }
        public class EffectDisplay
        {
            public EffectDisplay()
            {

            }
            public EffectDisplay(Sprite sprite, string name, string description, string index, params string[] additionals)
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
            public string[] Additionals;

        }
        public static EffectTemplate GetEffectTemplateByID(string effectID, byte level)
        {
            EffectDatabase effectDatabase = new EffectDatabase(level);
            effectDatabase.Effects.TryGetValue(effectID, out EffectTemplate databaseEffect);
            return databaseEffect;
        }
        ///<summary>
        ///Adds effect to the specified entity by template.
        ///</summary>
        public static void AddEffect(Entity applyEntity, string id, byte level, float duration)
        {
            EffectDatabase effectDatabase = new EffectDatabase(level);
            if(effectDatabase.Effects.ContainsKey(id) == false) 
            {
                Debug.LogWarning("Effect with id " + id + " doesn't exist.");
                return;
            }
            effectDatabase.Effects.TryGetValue(id, out EffectTemplate databaseEffect);
            
            AddEffect(applyEntity, duration, false, out byte _id, databaseEffect.StateEffects);
            AttachVisualAttrib(applyEntity, _id, databaseEffect.TemplateDisplay.Name, databaseEffect.TemplateDisplay.Description, databaseEffect.TemplateDisplay.Index, databaseEffect.TemplateDisplay.Sprite, databaseEffect.TemplateDisplay.Additionals);

        }
    }
}
