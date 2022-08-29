using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace Pay.UI
{
    public static class UIManager
    {
        ///<summary>
        ///Creates id that can be used to access all the specified UI elements (not removing individual specified id). 
        ///</summary>
        public static void CreateBundle(UIHolder holder, out byte id, params byte[] UIIDs)
        {
            id = GetID(holder);
            holder.BundleBuffer.Add(id, UIIDs);
        }
        public static bool ContainsId(UIHolder holder, byte id) => holder.InstantiatedUI.ContainsKey(id) || holder.BundleBuffer.ContainsKey(id);
        ///<summary>Removes UI (or all the bundled UI if specified id is a bundle).</summary>
        public static void RemoveUI(UIHolder holder, ref byte id)
        {
            bool containsUI = holder.InstantiatedUI.ContainsKey(id);
            bool containsBundle = holder.BundleBuffer.ContainsKey(id);
            if(containsUI && !containsBundle) RemoveSingle(holder, ref id);
            else if(containsBundle && !containsUI) RemoveBundle(holder, ref id);
            id = 0;
        }
        private static void RemoveSingle(UIHolder holder, ref byte id)
        {
            holder.InstantiatedUI.TryGetValue(id, out UIObject element);
            if(element == null) return;
            element.OnRemove();
            holder.InstantiatedUI.Remove(id);
        }
        private static void RemoveBundle(UIHolder holder, ref byte id)
        {
            holder.BundleBuffer.TryGetValue(id, out byte[] bundledID);
            for(int i = 0; i < bundledID.Length; i++) RemoveSingle(holder, ref bundledID[i]);
            holder.BundleBuffer.Remove(id);
        }
        internal static byte GetID(UIHolder holder) => Pay.Functions.Math.GetUniqueByte(Pay.Functions.Generic.CombineArrays(holder.BundleBuffer.Keys.ToArray(), holder.InstantiatedUI.Keys.ToArray()), 0);

        public static class Text
        {
            public static void CreateText(UIHolder holder, Canvas canvas, string text, TextConfiguration textConfiguration, float duration, float fadeIn, float fadeOut, out byte id, params Pay.UI.Options.TransformProperty[] properties)
            {
                GameObject obj = MonoBehaviour.Instantiate(holder.TextObject.gameObject, canvas.transform);
                obj.AddComponent<UITransform>();
                UnityEngine.UI.Text currentText = obj.GetComponent<UnityEngine.UI.Text>();
                currentText.text = text;
                currentText.font = textConfiguration.Font;
                currentText.color = textConfiguration.Color;
                currentText.lineSpacing = textConfiguration.LineSpacing;
                id = GetID(holder);
                TextObject textObject = new TextObject(holder, id, currentText, fadeIn, duration, fadeOut);
                
                holder.InstantiatedUI.Add(id, textObject);
                UIObject ui = textObject;
                UIManager.ActivateProperties(properties, ui);
                textObject = (TextObject)ui;
            }
        }
        public static class Indicator
        {
            public static void CreateIndicator(UIHolder holder, Canvas canvas, Pay.UI.Indicator indicator, out byte id, params Pay.UI.Options.UIProperty[] properties)
            {
                Image indicatorImage = new GameObject("Indicator", typeof(UITransform)).AddComponent<Image>();
                indicatorImage.type = Image.Type.Filled;
                indicatorImage.fillMethod = indicator.fillMethod;
                indicatorImage.sprite = indicator.sprite;
                indicatorImage.transform.SetParent(canvas.transform);
                indicatorImage.transform.localPosition = Vector3.zero;
                indicatorImage.transform.localScale = Vector3.one;
                id = GetID(holder);
                IndicatorObject indicatorObject = new IndicatorObject(holder, id, indicatorImage);
                holder.InstantiatedUI.Add(id, indicatorObject);
                UIObject ui = indicatorObject;
                ActivateProperties(properties, ui);
                indicatorObject = (IndicatorObject)ui;
            }
            public static void UpdateIndicator(UIHolder holder, byte id, float currentAmount, float maxAmount, params Pay.UI.Options.IndicatorProperty[] properties)
            {
                holder.InstantiatedUI.TryGetValue(id, out UIObject ui);
                if(ui == null) return;
                IndicatorObject indicatorObject = (IndicatorObject)ui;
                indicatorObject.IndicatorValue = currentAmount / maxAmount;
                indicatorObject.indicatorObject.fillAmount = indicatorObject.IndicatorValue;
                ActivateProperties(properties, ui);
                indicatorObject = (IndicatorObject)ui;
            }
            public static void RegisterIndicator(UIHolder holder, Image indicator, out byte id)
            {
                id = GetID(holder);
                IndicatorObject indicatorObject = new IndicatorObject(holder, id, indicator);
                holder.InstantiatedUI.Add(id, indicatorObject);
            }
        }
        private static void ActivateProperties(Pay.UI.Options.UIProperty[] properties, UIObject propertyUI)
        {
            foreach(Pay.UI.Options.UIProperty property in properties) property.Action?.Invoke(propertyUI);
        }
    }
    public abstract class UIObject 
    {
        public byte Id {get; private set;}
        public UIHolder Holder{get; private set;}
        public abstract GameObject GetObject();
        public virtual void OnRemove() => MonoBehaviour.Destroy(GetObject());
        public UIObject(UIHolder holder, byte id)
        {
            Id = id;
            Holder = holder;
        }
    }
    
    public class IndicatorObject : UIObject
    {
        public IndicatorObject(UIHolder holder, byte id, Image indicator) : base(holder, id) => indicatorObject = indicator;
        public Image indicatorObject {get; private set;}
        public float IndicatorValue {get; set;}
        public override GameObject GetObject() => indicatorObject.gameObject;
    }
    
    public class TextObject : UIObject
    {
        public TextObject(UIHolder holder, byte id, UnityEngine.UI.Text text, float fadeIn, float duration, float fadeOut) : base(holder, id)
        {
            Text = text;
            FadeInDuration = fadeIn;
            FadeOutDuration = fadeOut;
            Duration = duration;
        }
        public UnityEngine.UI.Text Text{get; private set;}
        public float FadeInDuration{get; private set;}
        public float Duration {get;private set;}
        public float FadeOutDuration{get; private set;}
        internal float curFadeInDuration;
        internal float curDuration;
        internal float curFadeOutDuration;
        public override GameObject GetObject() => Text.gameObject;
    }
    public class ImageObject : UIObject
    {
        public Image Object;
        public override GameObject GetObject() => Object.gameObject;
        public ImageObject(UIHolder holder, byte id, Image image) : base(holder, id) => Object = image;
    }
    
    [System.Serializable]
    public class TextConfiguration
    {
        public TextConfiguration(Font font, Color color, float lineSpacing)
        {
            Font = font;
            Color = color;
            LineSpacing = lineSpacing;
        }
        public Font Font;
        public Color Color;
        public float LineSpacing;
    }
    
    public class ChangeParameter
    {
        public string ChangedValue;
        public ChangeParameter(Color TextColor, string Parameter)
        {
            string returnValue = Parameter;
            returnValue = Pay.Functions.String.SetSides(returnValue, "<color=#" + ColorUtility.ToHtmlStringRGBA(TextColor) + ">", "</color>");
            
            ChangedValue = returnValue;
        }
    }
    
}