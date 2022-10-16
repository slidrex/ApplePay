using System.Collections;
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
        RoomObjectSpawn(room);
        for(int i = 0; i < room.MarkList.Count; i++)
        {
            room.ApplyMark(room.MarkList[i]);
        }
    }
    public static void ApplyMark(this Room room, RoomMark mark)
    {
        switch(mark.MarkType)
        {
            case MarkDatabase.MarkType.Mob:
                MobMark mobMark = (MobMark)mark;
                room.StartCoroutine(RoomMobSpawn(room, mobMark.SpawnDelay, mobMark.MinSpawnInterval, mobMark.MaxSpawnInterval,Random.Range(mobMark.MinMobCount, mobMark.MaxMobCount), room.MobList));
            break;
        }
    }
    private static IEnumerator RoomMobSpawn(Room room, float spawnDelay, float minSpawnInterval, float maxSpawnInterval, int mobCount, SpawnMob[] Mob)
    {
        yield return new WaitForSeconds(spawnDelay);
        for(int i = 0; i < mobCount; i++)
        {
            if(room.MobCountLimit <= i) break;
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, 100);
                int index = Random.Range(0, Mob.Length);
                if(Mob[index].SpawnChance > rand)
                {
                    MonoBehaviour.Instantiate(Mob[index].Mob, room.GetRandomRoomSpace(),Quaternion.identity);
                instantiated = true;
                }
            }
        }
    }
    private static void RoomObjectSpawn(Room room)
    {
        int objectCount = Random.Range(room.MinObjectsCount, room.MaxObjectsCount);
        for(int i = 0; i < objectCount; i++)
        {
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, room.EnvironmentObjectList.Count);
                if(room.EnvironmentObjectList[rand].SpawnChance >= Random.Range(0f, 1f))
                {
                    MonoBehaviour.Instantiate(room.EnvironmentObjectList[rand].Object, room.GetRandomRoomSpace(), Quaternion.identity);
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