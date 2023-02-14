using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionScaler : MonoBehaviour
{
    [SerializeField] private RectTransform nameTag;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        StaticCoroutine.ExecuteOnEndOfFrame( () => rect.sizeDelta = new Vector2(nameTag.sizeDelta.x, rect.sizeDelta.y));
    }
    
}
