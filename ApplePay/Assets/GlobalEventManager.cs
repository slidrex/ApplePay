using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
   public static System.Action OnActionActivated;
   public static void UpdateMobLists()
   {
        OnActionActivated?.Invoke();
   }
}
