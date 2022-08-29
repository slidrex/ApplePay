using UnityEngine;
[CreateAssetMenu(menuName = "Marks/New Mob Mark")]
public class MobMark : RoomMark
{
    public byte MinMobCount;
    public byte MaxMobCount;
    public float SpawnDelay;
    public float MinSpawnInterval;
    public float MaxSpawnInterval;

}
