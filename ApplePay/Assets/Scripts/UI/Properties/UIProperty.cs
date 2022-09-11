using UnityEngine;
namespace Pay.UI.Options
{
public class UIProperty  
{
    public System.Action<UIElement> Action;
    public UIProperty(System.Action<UIElement> action) => Action = action;
}
public class TransformProperty : UIProperty 
{
    public TransformProperty(System.Action<UIElement> action) : base(action) {}
}
public class IndicatorProperty : UIProperty 
{
    public IndicatorProperty(System.Action<UIElement> action) : base(action) {}
}

namespace Transform
{
public class DynamicProperty
{
    public static TransformProperty Position(Vector3 beginPos, Vector3 endPos,  bool returnBack, float speed) => new TransformProperty((UIElement element) => AssignTranformBehaviour(beginPos, endPos, returnBack, speed, TransformType.Position, element));
    public static TransformProperty Rotation(Vector3 beginRotation,Vector3 endRotation, bool returnBack, float speed) => new TransformProperty((UIElement element) => AssignTranformBehaviour(beginRotation, endRotation, returnBack, speed, TransformType.Rotation, element));
    public static TransformProperty LocalScale(Vector3 beginScale, Vector3 endScale, bool returnBack, float speed) => new TransformProperty((UIElement element) => AssignTranformBehaviour(beginScale, endScale, returnBack, speed, TransformType.LocalScale, element));
    private static void AssignTranformBehaviour(Vector3 beginValue, Vector3 endValue, bool returnBack, float speed, TransformType transformType, UIElement element)
    {
        TransformBehaviour transformBehaviour = new TransformBehaviour();
        transformBehaviour.startValue = beginValue;
        transformBehaviour.endValue = endValue;
        transformBehaviour.returnBack = returnBack;
        transformBehaviour.speed = speed;
        element.GetObject().GetComponent<UITransform>().AddState(transformType, transformBehaviour);
    }
}
public static class StaticProperty
{
    public static TransformProperty Parent(UnityEngine.Transform transform) => new TransformProperty((UIElement ui) => ui.GetObject().transform.SetParent(transform));
    public static TransformProperty Position(Vector3 position) => new TransformProperty((UIElement ui) => ui.GetObject().transform.position = position);
    public static TransformProperty Rotation(Vector3 rotation) => new TransformProperty((UIElement ui) => ui.GetObject().transform.rotation = Quaternion.Euler(rotation));
    public static TransformProperty LocalScale(Vector3 localScale) => new TransformProperty((UIElement ui) => ui.GetObject().transform.localScale = localScale);
}
public enum TransformType
{
    Position,
    Rotation,
    LocalScale
}
}
public static class IndicatorSettings
{
    ///<summary>
    ///Automatically removes indicator when its value reaches 100%.
    ///</summary>
    public static IndicatorProperty AutoRemove() 
    {
        System.Action<UIElement> action = delegate(UIElement element)
        {
            IndicatorObject indicatorObject = (IndicatorObject)element;
            if(indicatorObject.IndicatorValue >= 1)
            {
                Pay.UI.UIManager.RemoveUI(element);
            }
        };
        
        return new IndicatorProperty(action);
    }
    ///<summary>
    ///Reverses display value of indicator.
    ///</summary>
    public static IndicatorProperty ReverseAmount()
    {
        System.Action<UIElement> uiObject = delegate(UIElement element)
        {
            IndicatorObject indicatorObject = (IndicatorObject)element;
            indicatorObject.indicatorObject.fillAmount = 1 - indicatorObject.IndicatorValue;
        };
        return new IndicatorProperty(uiObject);
    }
}

}
