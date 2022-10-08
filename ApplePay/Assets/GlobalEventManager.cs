public static class GlobalEventManager
{
   public static System.Action OnActionActivated;
   public static void UpdateMobLists() 
   {
      RoomDefiner.RoomEntityListUpdater();
      OnActionActivated?.Invoke();
   }
}
