using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private MarkDatabase markDatabase;
    internal struct walker
    {
        internal walker(Vector2 position)
        {
            this.position = position;
            lastDirection = Pay.Functions.Math.GetRandomCrossVector();
        }
        internal Vector2 position;
        internal Vector2 lastDirection { get; private set; }
        internal void Move(Vector2 direction)
        {
            position += direction;
            lastDirection = direction;
        }
    }
    [SerializeField] private RandRoom[] spawnRooms;
    private List<walker> walkers = new List<walker>();
    private Vector2 zeroRoomPos;
    public int RoomCount = 1;
    [Header("Grid generation settings")]
    private Dictionary<Vector2, Room> FilledCells = new Dictionary<Vector2, Room>();
    [SerializeField] private float cellSize = 50f;
    [SerializeField] private byte StartGridComplexity = 1, GridComplexityLimit = 1;
    [SerializeField, Range(0, 1f)] private float GridComplexityIncreaseRate;
    [Range(0, 1)] public float chanceToChangeDirection;
    [Header("Spawn Setup")]
    [SerializeField, Tooltip("Objects that can be spawned in the first room.")] private List<GameObject> setupObjects = new List<GameObject>();
    [Header("Contracts")]
    [SerializeField] private RoomContract[] ContractRooms;
    private Dictionary<int, SpawnerRoom> contractedRooms = new Dictionary<int, SpawnerRoom>();
    private int spawnedRoomCount;
    private void Start()
    {
        Setup();
        GridGeneration();
        DoorsSetup();
        SetupObjects();
        FillMarks();
    }
    private void Setup() 
    {
        FillContracts();
        if(spawnedRoomCount < RoomCount) SpawnRoom(Vector2.zero, 0);
        for(int i = 0; i < StartGridComplexity; i++) SpawnWalker();
    }
    private void FillMarks()
    {
        foreach(Room room in FilledCells.Values) FillMark(room);
    }
    private void SpawnWalker() => walkers.Add(new walker(Vector2.zero));
    private void GridGeneration() 
    {
        while(spawnedRoomCount < RoomCount)
        {
            for(int j = 0; j < walkers.Count; j++)
            {
                walker thiswalker = walkers[j];
                bool isMoved = false;
                while(isMoved == false)
                {
                    Vector2 moveVector = Random.Range(0f, 1f) < chanceToChangeDirection ? Pay.Functions.Math.GetRandomCrossVector() : thiswalker.lastDirection;;
                    thiswalker.Move(moveVector);
                    
                    if(FilledCells.ContainsKey(thiswalker.position) == false)
                        isMoved = true;
                    walkers[j] = thiswalker;
                }

            }
            for(int i = 0; i < walkers.Count; i++) 
            {
                walker thisWalker = walkers[i];
                if(spawnedRoomCount >= RoomCount) break;
                if(FilledCells.ContainsKey(thisWalker.position) == false)
                {
                    SpawnRoom(thisWalker.position, spawnedRoomCount);
                    walkers[i] = thisWalker;
                }
            }
            if(Random.Range(0f, 1f) < GridComplexityIncreaseRate && walkers.Count < GridComplexityLimit) SpawnWalker();
        }
    }
    private void FillContracts()
    {
        for(int i = 0; i < ContractRooms.Length; i++)
        {
            contractedRooms.Add((byte)Random.Range(ContractRooms[i].MinRoom, ContractRooms[i].MaxRoom), ContractRooms[i].SpawnerRoom);
        }
    }
    private Room SpawnRoom(Vector2 walkerPosition, int spawnedRoomIndex)
    {
        if(spawnRooms.Length == 0) throw new System.Exception("No rooms is specified.");
        byte[] roomChances = spawnRooms.Select(x => x.spawnRate).ToArray();
        Pay.Functions.Generic.BubbleSort(false, ref roomChances);
        byte min = roomChances[0];
        byte max = roomChances[roomChances.Length - 1];
        bool spawned = false;
        Vector2 spawnPosition = zeroRoomPos + walkerPosition  * cellSize;
        while(spawned == false)
        {
            byte rotation = 0;
            if(contractedRooms.ContainsKey(spawnedRoomIndex))
            {
                contractedRooms.TryGetValue(spawnedRoomIndex, out SpawnerRoom room);
                if(room.availableRotations.Length != 0) rotation = room.availableRotations[Random.Range(0, room.availableRotations.Length)];
                contractedRooms.Remove(spawnedRoomIndex);
                return InstantiateRoom(room.room, rotation, spawnPosition , walkerPosition);
            }
            
            byte random = (byte)Random.Range(min, max + 1);
            byte index = (byte)Random.Range(0, spawnRooms.Length);
            if(spawnRooms[index].spawnRate >= random)
            {
                spawned = true;
                if(spawnRooms[index].SpawnerRoom.availableRotations.Length != 0) rotation = spawnRooms[index].SpawnerRoom.availableRotations[Random.Range(0, spawnRooms[index].SpawnerRoom.availableRotations.Length)];
                return InstantiateRoom(spawnRooms[index].SpawnerRoom.room, rotation, spawnPosition, walkerPosition);
            }
        }
        return null;
    }
    private Room InstantiateRoom(Room room, byte rotations, Vector2 spawnPosition, Vector2 walkerPosition)
    {
        Room obj = Instantiate(room.gameObject, spawnPosition, Quaternion.identity).GetComponent<Room>();
        FilledCells.Add(walkerPosition, obj);
        obj.transform.SetParent(transform);
        obj.transform.eulerAngles = obj.transform.eulerAngles - Vector3.forward * rotations * 90f;
        DoorBehaviour[] doors = obj.GetComponentsInChildren<DoorBehaviour>();
        foreach(DoorBehaviour door in doors) door.SwapDirection(rotations);
        spawnedRoomCount++;
        return obj.GetComponent<Room>();
    }
    private void FillMark(Room room)
    {
            for(int i = 0; i < room.MarkList.Count; i++)
            {
                while(room.MarkList[i] == null)
                {
                    
                    int index = Random.Range(0, markDatabase.MarkList.Count);
                    if(markDatabase.MarkList[index].SpawnChance >= Random.Range(1, 101))
                    {
                        room.MarkList[i] = markDatabase.MarkList[index].Mark;
                        break;
                    }
                    
                }
            }
    }
    private void DoorsSetup()
    {
        for(int i = 0; i < FilledCells.Count; i++)
        {
            DoorBehaviour[] unfilledDoors = FilledCells.ElementAt(i).Value.GetComponentsInChildren<DoorBehaviour>().Where(x => x.ConnectedDoor == null).ToArray();
            foreach(DoorBehaviour door in unfilledDoors)
            {
                Vector2 direction = door.Direction;
                bool isRoomExist = FilledCells.TryGetValue(FilledCells.ElementAt(i).Key + direction, out Room wrappedRoom);
                if(isRoomExist)
                {
                    DoorBehaviour suitableDoor = wrappedRoom.GetComponentsInChildren<DoorBehaviour>().FirstOrDefault(x => x.ConnectedDoor == null && x.Direction + door.Direction == Vector2.zero);
                    
                    if(suitableDoor == null)
                    {
                        Destroy(door.gameObject);
                        continue;
                    }
                    suitableDoor.ConnectedDoor = door;
                    door.ConnectedDoor = suitableDoor;
                }
                else Destroy(door.gameObject);
            }
        }
    }
    private void SetupObjects()
    {
        for(int i = 0; i < setupObjects.Count; i++) setupObjects[i].transform.position = FilledCells.ElementAt(0).Value.gameObject.transform.position;
    }
    [System.Serializable]
    public struct RandRoom
    {
        public SpawnerRoom SpawnerRoom;
        [Range(0, 100)] public byte spawnRate;
    }
    [System.Serializable]
    public struct SpawnerRoom
    {
        public Room room;
        [Tooltip("1 turn = rotate 90 degrees clockwise"), Range(0, 3)] public byte[] availableRotations;
    }
    [System.Serializable]
    public struct RoomContract
    {
        public SpawnerRoom SpawnerRoom;
        public byte MinRoom;
        public byte MaxRoom;
    }
}