using System.Collections.Generic;
using UnityEngine;
public class Room : MonoBehaviour
{
    [SerializeField, Tooltip("Specified space is free for spawn objects.")] private RoomBound[] FreeRoomSpace;
    [SerializeField, Tooltip("Used for checking if object inside the room.")] public RoomBound RoomConfiners;
    public List<Creature> EntityList = new List<Creature>();
    public List<RoomMark> MarkList = new List<RoomMark>();
    [Header("Environment settings")]
    public List<EnvironmentObject> EnvironmentObjectList = new List<EnvironmentObject>();
    public byte MinObjectsCount, MaxObjectsCount;
    [HideInInspector] public bool isActive;
    public RoomSpawner spawner { get; set; }
    [Header("Mob Room")]
    public byte MobCountLimit;
    public SpawnMob[] MobList;
    public byte MinStageCount, MaxStageCount;
    private void Awake()
    {
        SetupBounds();
        LinkDoors();
    }
    private void SetupBounds()
    {
        for(int i = 0; i < FreeRoomSpace.Length; i++) FreeRoomSpace[i].RelatedTransform = transform;
        RoomConfiners.RelatedTransform = transform;
    }
    private void LinkDoors()
    {
        DoorBehaviour[] doors =  GetComponentsInChildren<DoorBehaviour>();
        foreach(DoorBehaviour door in doors) door.AttachedRoom = this;
    }
    public void DefineRoom()
    {
        isActive = true;
        RoomDefiner.ActivateRoomMarks(this);
    }
    public bool IsInsideRoom(Vector2 position) =>  RoomConfiners.IsInsideBound(position);
    public Vector2 GetRandomFreeRoomSpace() => FreeRoomSpace[Random.Range(0, FreeRoomSpace.Length)].GetRandomSpace();
    [System.Serializable]
    public struct EnvironmentObject
    {
        public GameObject Object;
        [Range(0, 1f)] public float SpawnChance;
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        for(int i = 0; i < FreeRoomSpace.Length; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube((Vector2)FreeRoomSpace[i].GetOffset(), FreeRoomSpace[i].localScale);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)RoomConfiners.GetOffset(), RoomConfiners.localScale);
    }
    [System.Serializable]
    public struct RoomBound
    {
        [SerializeField] internal Vector2 localScale;
        [SerializeField] internal Vector2 offset;
        public Transform RelatedTransform;
        public Vector2 GetScale() => localScale * (Vector2)RelatedTransform.lossyScale;
        public Vector2 GetOffset() => (Vector3)offset;
        public Vector2 GetZeroBoundPosition() => offset + (Vector2)RelatedTransform.transform.position;
        ///<summary>
        ///<code> Gets the corners of the bounds in the following order: </code>
        ///<code> 0 - Top Left</code>
        ///<code> 1 - Top Right </code>
        ///<code> 2 - Bottom Right </code>
        ///<code> 3 - Bottom Left </code>
        ///</summary>
        public Vector2[] GetCorners() 
        {
            Vector2[] sourceCorners = new Vector2[4]
            {
                new Vector2(GetZeroBoundPosition().x - GetScale().x / 2, GetZeroBoundPosition().y + GetScale().y / 2),
                GetZeroBoundPosition() + GetScale() / 2,
                new Vector2(GetZeroBoundPosition().x + GetScale().x / 2, GetZeroBoundPosition().y - GetScale().y / 2),
                GetZeroBoundPosition() - GetScale() / 2
            };
            return sourceCorners;
        }
        public bool IsInsideBound(Collider2D collider)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(GetZeroBoundPosition(), GetScale(), RelatedTransform.eulerAngles.z);
            foreach(Collider2D _collider in colliders)
            {
                if(_collider == collider)
                    return true;
            }
            return false;
        }
        public bool IsInsideBound(Vector2 position)
        {
            Vector2[] corners = GetCorners();
            Vector2 fixedPos = Pay.Functions.Math.RotateVector(position - GetZeroBoundPosition(), -RelatedTransform.eulerAngles.z) + GetZeroBoundPosition();
            
            bool isInsideBound = fixedPos.x <= corners[1].x && fixedPos.x >= corners[0].x && corners[0].y >= fixedPos.y && corners[2].y <= fixedPos.y;
            return isInsideBound;
        }
        ///<summary> Gets random space within the bound. </summary>
        public Vector2 GetRandomSpace()
        {
            Vector2[] corners = GetCorners();
            
            return Pay.Functions.Math.RotateVector(new Vector2(Random.Range(corners[0].x, corners[1].x), Random.Range(corners[2].y, corners[1].y)) - GetZeroBoundPosition(), RelatedTransform.eulerAngles.z) + GetZeroBoundPosition();
        }
    }
}