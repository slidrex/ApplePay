public static class RoomRenderer
{
    public static void RenderRoom(RoomSpawner spawner, params Room[] rooms)
    {
        foreach(Room room in spawner.ActiveLevelRooms)
        {
            foreach(Creature entity in room.EntityList)
            {
                entity.gameObject.SetActive(false);
            }
        }
        foreach(Room room in rooms)
        {
            foreach(Creature entity in room.EntityList)
            {
                entity.gameObject.SetActive(true);
            }
            room.gameObject.SetActive(true);
        }
    }
}
