using System.Collections;
using UnityEngine;

public class RoomDefiner : MonoBehaviour
{
    private void Awake() => GlobalEventManager.OnActionActivated += RoomMobListUpdater;
    private void OnDestroy() => GlobalEventManager.OnActionActivated -= RoomMobListUpdater;
    private void RoomMobListUpdater()
    {
        Creature[] entities = FindObjectsOfType<Creature>();
        Room[] rooms = FindObjectsOfType<Room>();
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
    private IEnumerator RoomMobSpawn(float SpawnDelay, float MinSpawnInterval, float MaxSpawnInterval, int MobCount, SpawnMob[] Mob, Room room)
    {
        yield return new WaitForSeconds(SpawnDelay);
        for(int i = 0; i < MobCount; i++)
        {
            if(room.MobCountLimit <= i) break;
            yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(0, 100);
                int index = Random.Range(0, Mob.Length);
                if(Mob[index].SpawnChance > rand){
                    Instantiate(Mob[index].Mob, room.GetRandomRoomSpace(),Quaternion.identity);
                instantiated = true;
            }
        }
        }
    }
    private void RoomObjectSpawn(Room room)
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
                    Instantiate(room.EnvironmentObjectList[rand].Object, room.GetRandomRoomSpace(), Quaternion.identity);
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