using System.Collections;
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
    public static void ExecuteOnEndOfFrame(System.Action action) => BeginCoroutine(EndOfFrameAction(action));
    private static IEnumerator EndOfFrameAction(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        action.Invoke();
    }
}