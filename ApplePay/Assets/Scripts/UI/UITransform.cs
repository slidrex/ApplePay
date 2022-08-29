using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pay.UI.Options.Transform;
public class UITransform : MonoBehaviour
{
    
    private Dictionary<TransformType, TransformBehaviour> dynamicStates = new Dictionary<TransformType, TransformBehaviour>();
    private MoveDirection currentDirection;
    private void Start() 
    {
        for(int i = 0; i < dynamicStates.Count; i++) StartTranformUI(dynamicStates.ElementAt(i).Key, dynamicStates.ElementAt(i).Value);
    }
    private void Update()
    {
        for(int i = 0; i < dynamicStates.Count; i++) TransformUI(dynamicStates.ElementAt(i).Value, dynamicStates.ElementAt(i).Key, transform, i);
    }
    public void AddState(TransformType transformType, TransformBehaviour transformBehaviour) => dynamicStates.Add(transformType, transformBehaviour);
    private void TransformUI(TransformBehaviour transformBehaviour, TransformType transformType, Transform applyTrasnform, int dictIndex)
    {
        Vector3 destinationPoint = currentDirection == MoveDirection.Forward ? transformBehaviour.endValue : transformBehaviour.startValue;

        transformBehaviour.currentValue = Vector3.MoveTowards(transformBehaviour.currentValue, destinationPoint, transformBehaviour.speed * Time.deltaTime);

        SetValue(transformType, transformBehaviour, applyTrasnform, dynamicStates);
        if(transformBehaviour.currentValue == destinationPoint)
        {
            if(currentDirection == MoveDirection.Forward && transformBehaviour.returnBack) {currentDirection = MoveDirection.Back;return;}
            StartTranformUI(transformType, transformBehaviour);
        }

    }
    private void StartTranformUI(TransformType transformType, TransformBehaviour transformBehaviour)
    {
        currentDirection = MoveDirection.Forward;
        transformBehaviour.currentValue = transformBehaviour.startValue;
        SetValue(transformType, transformBehaviour, transform, dynamicStates);
    }
    private void SetValue(TransformType transformType, TransformBehaviour value, Transform applyTransform, Dictionary<TransformType, TransformBehaviour> workDict)
    {
        switch(transformType)
        {
            case TransformType.Position:
            applyTransform.position = value.currentValue;
            break;
            case TransformType.Rotation:
            applyTransform.eulerAngles = value.currentValue;
            break;
            case TransformType.LocalScale:
            applyTransform.localScale = value.currentValue;
            break;
        }
        workDict.Remove(transformType);
        workDict.Add(transformType, value);
    }
    private enum MoveDirection
    {
        Forward,
        Back
    }
}
public struct TransformBehaviour
{
    public Vector3 startValue;
    public Vector3 endValue;
    public bool returnBack;
    public float speed;
    internal Vector3 currentValue;
}