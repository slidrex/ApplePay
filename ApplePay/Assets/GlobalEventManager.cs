public static class GlobalEventManager
{
   public static System.Action OnActionActivated;
   public static void UpdateMobLists() => OnActionActivated?.Invoke();
}
