using System.Collections;
using UnityEngine;

public class RoomDefiner : MonoBehaviour
{
    private void Awake() => GlobalEventManager.OnActionActivated += RoomMobListUpdater;
    private void OnDestroy() => GlobalEventManager.OnActionActivated -= RoomMobListUpdater;
    private void RoomMobListUpdater()
    {
        Creature[] entities = FindObjectsOfType<Creature>();
        Room[] roomManagers = FindObjectsOfType<Room>();
        foreach(Room room in roomManagers)
        {
            foreach(Creature entity in entities)
            {
                if(room.IsInsideRoom(entity.transform.position)  && !entity.isDead)
                {
                    if(room.EntityList.Contains(entity) == false)
                    {
                        room.EntityList.Add(entity);
                        entity.CurrentRoom = room;
                    }
                }
                else if(room.EntityList.Contains(entity))
                {
                    room.EntityList.Remove(entity);
                    entity.CurrentRoom = null;
                }
                room.EntityList.RemoveAll(s => s == null);
            }
        }
    }
        
    public void RoomDefine(Room room)
    {
        RoomObjectSpawn(room);
        for(int i = 0; i < room.MarkList.Count; i++)
        {
            ApplyMark(room.MarkList[i], room);
        }
        
    }
    public void ApplyMark(RoomMark mark, Room room)
    {
        switch(mark.MarkType)
        {
            case MarkDatabase.MarkType.Mob:
                MobMark mobMark = (MobMark)mark;
                StartCoroutine(RoomMobSpawn(mobMark.SpawnDelay, mobMark.MinSpawnInterval, mobMark.MaxSpawnInterval,Random.Range(mobMark.MinMobCount, mobMark.MaxMobCount), room.MobList, room));
            break;
        }
    }
    private IEnumerator RoomMobSpawn(float SpawnDelay, float MinSpawnInterval, float MaxSpawnInterval, int MobCount, SpawnMob[] Mob, Room roomManager)
    {
        yield return new WaitForSeconds(SpawnDelay);
        for(int i = 0; i < MobCount; i++)
        {
            if(roomManager.MobCountLimit <= i) break;
            yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, 100);
                int index = Random.Range(0, Mob.Length);
                if(Mob[index].SpawnChance > rand){
                    Instantiate(Mob[index].Mob, roomManager.GetRandomRoomSpace(),Quaternion.identity);
                instantiated = true;
            }
        }
        }
    }
    private void RoomObjectSpawn(Room roomManager)
    {
        int objectCount = Random.Range(roomManager.MinObjectsCount, roomManager.MaxObjectsCount);
        for(int i = 0; i < objectCount; i++)
        {
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, roomManager.EnvironmentObjectList.Count);
                if(roomManager.EnvironmentObjectList[rand].SpawnChance >= Random.Range(0f, 1f))
                {
                    Instantiate(roomManager.EnvironmentObjectList[rand].Object, roomManager.GetRandomRoomSpace(), Quaternion.identity);
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