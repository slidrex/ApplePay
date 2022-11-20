using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Allows you to use coroutine without mono behaviour instance
///</summary>

public class StaticCoroutine : MonoBehaviour
{
    private static MonoBehaviour instance;
    private void Awake() => instance = this;
    public static void BeginCoroutine(IEnumerator coroutine) => instance.StartCoroutine(coroutine);
    public static void EndCoroutine(IEnumerator coroutine) => instance.StopCoroutine(coroutine);
}