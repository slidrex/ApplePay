using UnityEngine;
using UnityEngine.UI;

namespace Pay.UI
{
    public static class UIManager
    {
        ///<summary>
        ///Creates unique UI container that can be used to access all the specified UI elements (not removing individual specified UI containers). 
        ///</summary>
        public static UIElementBundle CreateBundle(params UIElement[] elements) => new UIElementBundle(elements);
        public static void RemoveUI(UIElement element) 
        {
            if(element == null || element.IsEmpty() || element.GetObject() == null) return;
            element.OnRemove();
        }
        public static void RemoveUI(UIElementBundle bundle)
        {
            foreach(UIElement element in bundle.Elements)
            {
                element.GetHolder().InstantiatedUI.Remove(element);
                element.OnRemove();
            }
        }
        public static class Text
        {
            public static void CreateText(UIHolder holder, Canvas canvas, string text, TextConfiguration textConfiguration, float duration, AnimationCurve alphaBehaviour, out TextObject container, params Pay.UI.Options.TransformProperty[] properties)
            {
                GameObject obj = MonoBehaviour.Instantiate(holder.TextObject.gameObject, canvas.transform);
                obj.AddComponent<UITransform>();
                UnityEngine.UI.Text currentText = obj.GetComponent<UnityEngine.UI.Text>();
                currentText.text = text;
                currentText.font = textConfiguration.Font;
                currentText.color = textConfiguration.Color;
                currentText.lineSpacing = textConfiguration.LineSpacing;
                container = new TextObject(holder, currentText, duration, alphaBehaviour);
                
                holder.InstantiatedUI.Add(container);
                UIManager.ActivateProperties(properties, container);
            }
        }
        public struct TextField
        {
            public string Text;
            public TextConfiguration TextConfiguration;
            public TextField(Pay.UI.TextConfiguration configuration, string text)
            {
                Text = text;
                TextConfiguration = configuration;
            }
        }
        public static class Indicator
        {
            public static void CreateIndicator(UIHolder holder, Canvas canvas, Pay.UI.Indicator indicator, out IndicatorObject container, params Pay.UI.Options.UIProperty[] properties)
            {
                Image indicatorImage = new GameObject("Indicator", typeof(UITransform)).AddComponent<Image>();
                indicatorImage.type = Image.Type.Filled;
                indicatorImage.fillMethod = indicator.fillMethod;
                indicatorImage.sprite = indicator.sprite;
                indicatorImage.transform.SetParent(canvas.transform);
                indicatorImage.transform.localPosition = Vector3.zero;
                indicatorImage.transform.localScale = Vector3.one;
                
                container = new IndicatorObject(holder, indicatorImage);
                holder.InstantiatedUI.Add(container);
                ActivateProperties(properties, container);
            }
            public static void UpdateIndicator(IndicatorObject indicator, float currentAmount, float maxAmount, params Pay.UI.Options.IndicatorProperty[] properties)
            {
                indicator.IndicatorValue = currentAmount / maxAmount;
                indicator.indicatorObject.fillAmount = indicator.IndicatorValue;
                ActivateProperties(properties, indicator);
            }
            public static void RegisterIndicator(UIHolder holder, Image indicator, out IndicatorObject container)
            {
                container = new IndicatorObject(holder, indicator);
                holder.InstantiatedUI.Add(container);
            }
        }
        private static void ActivateProperties(Pay.UI.Options.UIProperty[] properties, UIElement propertyUI)
        {
            foreach(Pay.UI.Options.UIProperty property in properties) property.Action?.Invoke(propertyUI);
        }
    }
    public class UIElementBundle
    {
        public UIElement[] Elements { get; private set; }
        public void AddElements(params UIElement[] elements) => Elements = elements;
        ///<summary> Removes all the elements the bundle contains. </summary>
        public void RemoveBundle()
        {
            foreach(UIElement iObject in Elements) iObject.OnRemove();
            Elements = null;
        }
        public UIElementBundle(params UIElement[] elements) => AddElements(elements);
    }
    public abstract class UIElement
    {
        [SerializeField] protected UIHolder Holder;

        public abstract GameObject GetObject();
        ///<summary>Checks if the holder holder is attached. (Note that if a container has attached holder and it contains no UI it is NOT empty). </summary>
        public bool IsEmpty() => Holder == null;
        public UIHolder GetHolder() => Holder;
        public virtual void OnRemove() 
        {
            Holder.InstantiatedUI.Remove(this);
            Holder = null;
            MonoBehaviour.Destroy(GetObject());
        }
        public UIElement(UIHolder holder) => Holder = holder;
    }
    [System.Serializable]
    public class IndicatorObject : UIElement
    {
        public IndicatorObject(UIHolder holder, Image indicator) : base(holder) => indicatorObject = indicator;
        public Image indicatorObject {get; private set;}
        public float IndicatorValue {get; set;}
        public override GameObject GetObject() => indicatorObject == null ? null : indicatorObject.gameObject;
    }
    [System.Serializable]
    public class TextObject : UIElement
    {
        public TextObject(UIHolder holder, UnityEngine.UI.Text text, float duration, AnimationCurve alphaBehaviour) :  base(holder)
        {
            Text = text;
            AlphaBehaviour = alphaBehaviour;
            Duration = duration;
        }
        public UnityEngine.UI.Text Text { get; private set; }
        public AnimationCurve AlphaBehaviour { get; private set; }
        public float Duration { get; private set; }
        internal float curDuration;
        public override GameObject GetObject() => Text == null ? null : Text.gameObject;
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