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
            public static TextObject CreateText(UIHolder holder, Canvas canvas, string text, TextConfiguration textConfiguration, float fadeIn, float duration, float fadeOut, params Pay.UI.Options.TransformProperty[] properties)
            {
                AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(fadeIn, 1), new Keyframe(fadeIn + duration, 1), new Keyframe(fadeIn + duration + fadeOut, 0));
                
                UnityEngine.UI.Text obj = MonoBehaviour.Instantiate(holder.TextObject, canvas.transform);
                
                obj.text = text;
                obj.font = textConfiguration.Font;
                obj.color = textConfiguration.Color;
                obj.lineSpacing = textConfiguration.LineSpacing;
                TextObject container = new TextObject(holder, obj, duration + fadeIn + fadeOut, animationCurve);
                
                holder.InstantiatedUI.Add(container);
                UIManager.ActivateProperties(properties, container);
                return container;
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
        public static class Image
        {
            public static ImageObject CreateImage(UIHolder holder, Canvas canvas, UnityEngine.UI.Image image, params Pay.UI.Options.TransformProperty[] properties)
            {
                UnityEngine.UI.Image img = MonoBehaviour.Instantiate(image, canvas.transform);
                img.gameObject.AddComponent<UITransform>();
                ImageObject container = new ImageObject(holder, img);
                UIManager.ActivateProperties(properties, container);
                holder.InstantiatedUI.Add(container);
                return container;
            }
            public static ImageObject RegisterImage(UIHolder holder, UnityEngine.UI.Image image, params Pay.UI.Options.TransformProperty[] properties)
            {
                ImageObject container = new ImageObject(holder, image);
                container.GetObject().AddComponent<UITransform>();
                UIManager.ActivateProperties(properties, container);
                holder.InstantiatedUI.Add(container);
                return container;
            }
        }
        public static class Indicator
        {
            public static IndicatorObject CreateIndicator(UIHolder holder, Canvas canvas, Pay.UI.Indicator indicator, params Pay.UI.Options.UIProperty[] properties)
            {
                UnityEngine.UI.Image indicatorImage = new GameObject("Indicator", typeof(UITransform)).AddComponent<UnityEngine.UI.Image>();
                indicatorImage.type = UnityEngine.UI.Image.Type.Filled;
                
                indicatorImage.fillMethod = indicator.fillMethod;
                indicatorImage.sprite = indicator.sprite;
                indicatorImage.transform.SetParent(canvas.transform);
                indicatorImage.transform.localPosition = Vector3.zero;
                indicatorImage.transform.localScale = Vector3.one;
                
                IndicatorObject container = new IndicatorObject(holder, indicatorImage);
                holder.InstantiatedUI.Add(container);
                ActivateProperties(properties, container);
                return container;
            }
            public static void UpdateIndicator(IndicatorObject indicator, float currentAmount, float maxAmount, params Pay.UI.Options.IndicatorProperty[] properties)
            {
                indicator.IndicatorValue = currentAmount / maxAmount;
                indicator.indicatorObject.fillAmount = indicator.IndicatorValue;
                ActivateProperties(properties, indicator);
            }
            public static IndicatorObject RegisterIndicator(UIHolder holder, UnityEngine.UI.Image indicator)
            {
                IndicatorObject container = new IndicatorObject(holder, indicator);
                holder.InstantiatedUI.Add(container);
                return container;
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
    public class ImageObject : UIElement
    {
        public Image imageObject {get; private set;}
        public ImageObject(UIHolder holder, Image image) : base(holder) => imageObject = image;
        public override GameObject GetObject() => imageObject.gameObject;
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