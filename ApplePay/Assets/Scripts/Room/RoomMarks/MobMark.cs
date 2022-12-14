using UnityEngine;

[CreateAssetMenu(menuName = "Marks/New Mob Mark")]

public class MobMark : RoomMark
{
    public byte MinMobCount;
    public byte MaxMobCount;
    public float SpawnDelay;
    public float MinSpawnInterval;
    public float MaxSpawnInterval;
    public override void ApplyMark(Room room) => room.StartCoroutine(SpawnMobs(room));
    private System.Collections.IEnumerator SpawnMobs(Room room)
    {
        yield return new WaitForSecondsRealtime(SpawnDelay);
        int mobCount = Random.Range(MinMobCount, MaxMobCount);
        for(int i = 0; i < mobCount; i++)
        {
            if(room.MobCountLimit <= i) break;
            yield return new WaitForSeconds(Random.Range(MinSpawnInterval, MaxSpawnInterval));
            bool instantiated = false;
            while(!instantiated)
            {
                int rand = Random.Range(1, 101);
                int index = Random.Range(0, room.MobList.Length);
                if(room.MobList[index].SpawnChance >= rand)
                {
                    MonoBehaviour.Instantiate(room.MobList[index].Mob, room.GetRandomFreeRoomSpace(), Quaternion.identity);
                    instantiated = true;
                }
            }
        }
    }

}
