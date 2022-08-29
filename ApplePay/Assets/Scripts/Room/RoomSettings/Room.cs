using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Room : MonoBehaviour
{
    [ReadOnly] public List<Creature> EntityList = new List<Creature>();
    public List<RoomMark> MarkList = new List<RoomMark>();
    [Header("Environment settings")]
    public List<EnvironmentObject> EnvironmentObjectList = new List<EnvironmentObject>();
    public byte MinObjectsCount, MaxObjectsCount;
    [HideInInspector] public bool isActive;
    [Header("Room settings")]
    [SerializeField] private Transform minRoomPoint, maxRoomPoint;
    [Header("Mob Room")]
    public byte MobCountLimit;
    public SpawnMob[] MobList;
    public byte MinStageCount, MaxStageCount;
    private void Awake() => LinkDoors();
    private void LinkDoors()
    {
        DoorBehaviour[] doors =  GetComponentsInChildren<DoorBehaviour>();
        foreach(DoorBehaviour door in doors) door.AttachedRoom = this;
    }
    public void DefineRoom()
    {
        isActive = true;
        FindObjectOfType<RoomDefiner>().RoomDefine(this);
    }
    public bool IsInsideRoom(Vector2 position) => position.x >= minRoomPoint.position.x && position.y >= minRoomPoint.position.y && position.x <= maxRoomPoint.position.x && position.y <= maxRoomPoint.position.y;
    public Vector2 GetRandomRoomSpace() => new Vector2(Random.Range(minRoomPoint.position.x, maxRoomPoint.position.x), Random.Range(minRoomPoint.position.y, maxRoomPoint.position.y));
    [System.Serializable]
    public struct EnvironmentObject
    {
        public GameObject Object;
        [Range(0, 1f)] public float SpawnChance;
    }
}