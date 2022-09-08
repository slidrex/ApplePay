using UnityEngine;
namespace Pay.UI.Options
{
public class UIProperty  
{
    public System.Action<UIObject> Action;
    public UIProperty(System.Action<UIObject> action) => Action = action;
}
public class TransformProperty : UIProperty 
{
    public TransformProperty(System.Action<UIObject> action) : base(action) {}
}
public class IndicatorProperty : UIProperty 
{
    public IndicatorProperty(System.Action<UIObject> action) : base(action) {}
}

namespace Transform
{
public class DynamicProperty
{
    public static TransformProperty Position(Vector3 beginPos, Vector3 endPos,  bool returnBack, float speed) => new TransformProperty((UIObject ui) => AssignTranformBehaviour(beginPos, endPos, returnBack, speed, TransformType.Position, ui));
    public static TransformProperty Rotation(Vector3 beginRotation,Vector3 endRotation, bool returnBack, float speed) => new TransformProperty((UIObject ui) => AssignTranformBehaviour(beginRotation, endRotation, returnBack, speed, TransformType.Rotation, ui));
    public static TransformProperty LocalScale(Vector3 beginScale, Vector3 endScale, bool returnBack, float speed) => new TransformProperty((UIObject ui) => AssignTranformBehaviour(beginScale, endScale, returnBack, speed, TransformType.LocalScale, ui));
    private static void AssignTranformBehaviour(Vector3 beginValue, Vector3 endValue, bool returnBack, float speed, TransformType transformType, UIObject iObject)
    {
        TransformBehaviour transformBehaviour = new TransformBehaviour();
        transformBehaviour.startValue = beginValue;
        transformBehaviour.endValue = endValue;
        transformBehaviour.returnBack = returnBack;
        transformBehaviour.speed = speed;
        iObject.GetObject().GetComponent<UITransform>().AddState(transformType, transformBehaviour);
    }
}
public static class StaticProperty
{
    public static TransformProperty Parent(UnityEngine.Transform transform) => new TransformProperty((UIObject ui) => ui.GetObject().transform.SetParent(transform));
    public static TransformProperty Position(Vector3 position) => new TransformProperty((UIObject ui) => ui.GetObject().transform.position = position);
    public static TransformProperty Rotation(Vector3 rotation) => new TransformProperty((UIObject ui) => ui.GetObject().transform.rotation = Quaternion.Euler(rotation));
    public static TransformProperty LocalScale(Vector3 localScale) => new TransformProperty((UIObject ui) => ui.GetObject().transform.localScale = localScale);
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
    ///Automatically removes indicator if its value reaches 100%.
    ///</summary>
    public static IndicatorProperty AutoRemove() 
    {
        System.Action<UIObject> action = delegate(UIObject ui)
        {
            IndicatorObject indicatorObject = (IndicatorObject)ui;
            if(indicatorObject.IndicatorValue >= 1)
            {
                byte id = indicatorObject.Id;
                Pay.UI.UIManager.RemoveUI(indicatorObject.Holder, ref id);
            }
        };
        
        return new IndicatorProperty(action);
    }
    ///<summary>
    ///Reverses display value of indicator.
    ///</summary>
    public static IndicatorProperty ReverseAmount()
    {
        System.Action<UIObject> uiObject = delegate(UIObject ui)
        {
            IndicatorObject indicatorObject = (IndicatorObject)ui;
            indicatorObject.indicatorObject.fillAmount = 1 - indicatorObject.IndicatorValue;
        };
        return new IndicatorProperty(uiObject);
    }
}

}
