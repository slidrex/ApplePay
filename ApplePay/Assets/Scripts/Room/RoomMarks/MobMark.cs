using UnityEngine;

[CreateAssetMenu(menuName = "Marks/Room/New Mob Spawn Manual Mark")]

public class MobMark : ActionMark
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
                int index = Random.Range(0, room.MobList.Items.Length);
                if(room.MobList.Items[index].rate >= rand)
                {
                    MonoBehaviour.Instantiate(room.MobList.Items[index].item, room.GetRandomFreeRoomSpace(), Quaternion.identity);
                    instantiated = true;
                }
            }
        }
        room.OnMarkReleased(this);
    }

}
