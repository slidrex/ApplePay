using UnityEngine;

public static class RoomDefiner
{
    public static void RoomEntityListUpdater()
    {
        Creature[] entities = MonoBehaviour.FindObjectsOfType<Creature>();
        Room[] rooms = MonoBehaviour.FindObjectsOfType<Room>();
        foreach(Creature entity in entities) entity.CurrentRoom = null;
        foreach(Room room in rooms)
        {
            foreach(Creature entity in entities)
            {
                if(room.EntityList.Capacity > 0) room.EntityList.Clear();
                if(room.RoomConfiners.IsInsideBound(entity.transform.position) && !entity.isDead)
                {
                    room.EntityList.Add(entity);
                    entity.CurrentRoom = room;
                }
            }
        }
    }
    public static void ActivateRoomMarks(this Room room)
    {
        RoomObjectsSpawn(room);
        for(int i = 0; i < room.MarkList.Count; i++)
        {
            room.ApplyMark(room.MarkList[i]);
        }
    }
    
    public static void ApplyMark(this Room room, RoomMark mark) => mark.ApplyMark(room);

    private static void RoomObjectsSpawn(Room room)
    {
        int objectCount = Random.Range(room.MinObjectsCount, room.MaxObjectsCount);
        for(int i = 0; i < objectCount; i++)
        {
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, room.EnvironmentObjectList.Count);
                if(room.EnvironmentObjectList[rand].SpawnChance > Random.Range(0f, 1f))
                {
                    MonoBehaviour.Instantiate(room.EnvironmentObjectList[rand].Object, room.GetRandomFreeRoomSpace(), Quaternion.identity);
                    instantiated = true;
                }
            }
        }
    }
}
[System.Serializable]
public class SpawnMob
{
    public GameObject Mob;
    public float SpawnChance;
}