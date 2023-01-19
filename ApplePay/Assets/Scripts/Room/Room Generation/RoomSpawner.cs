using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomSpawner : MonoBehaviour
{
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
    public FloorLevelScenario Scenario;
    private Vector2 zeroRoomPos;
    private List<walker> walkers;
    [Header("Grid generation settings")]
    private Dictionary<Vector2, SpawnerRoom> FilledCells;

    [Header("Contracts")]
    private Dictionary<int, SpawnerRoom> contractedRooms;
    [HideInInspector] public System.Collections.Generic.List<Transform> StartObjects;
    private int spawnedRoomCount;
    [HideInInspector] public Transform RoomContainer;
    public SpawnerRoom[] ActiveLevelRooms { get; set; }
    private void Awake()
    {
        GenerateLevel();
    }
    public void GenerateLevel()
    {
        Setup();
        GridGeneration();
        DoorsSetup();
        SetupObjects();
        FillMarks();
    }
    private void Setup() 
    {
        spawnedRoomCount = 0;
        walkers = new List<walker>();
        StartObjects = new List<Transform>();
        FilledCells = new Dictionary<Vector2, SpawnerRoom>();
        contractedRooms = new Dictionary<int, SpawnerRoom>();
        RoomContainer = new GameObject("Room container").transform;
        ActiveLevelRooms = new SpawnerRoom[Scenario.RoomCount];
        
        if(Scenario == null) throw new System.NullReferenceException("Scenario hasn't been specified!");
        FillContracts();
        if(spawnedRoomCount < Scenario.RoomCount) SpawnRoom(Vector2.zero, 0);
        for(int i = 0; i < Scenario.StartGridComplexity; i++) SpawnWalker();
    }
    private void FillMarks()
    {
        foreach(SpawnerRoom room in FilledCells.Values) FillMark(room);
    }
    private void SpawnWalker() => walkers.Add(new walker(Vector2.zero));
    private void GridGeneration() 
    {
        while(spawnedRoomCount < Scenario.RoomCount)
        {
            for(int j = 0; j < walkers.Count; j++)
            {
                walker thiswalker = walkers[j];
                bool isMoved = false;
                while(isMoved == false)
                {
                    Vector2 moveVector = Random.Range(0f, 1f) < Scenario.ChangeDirectionRate ? Pay.Functions.Math.GetRandomCrossVector() : thiswalker.lastDirection;;
                    thiswalker.Move(moveVector);
                    
                    if(FilledCells.ContainsKey(thiswalker.position) == false)
                        isMoved = true;
                    walkers[j] = thiswalker;
                }

            }
            for(int i = 0; i < walkers.Count; i++) 
            {
                walker thisWalker = walkers[i];
                if(spawnedRoomCount >= Scenario.RoomCount) break;
                if(FilledCells.ContainsKey(thisWalker.position) == false)
                {
                    SpawnRoom(thisWalker.position, spawnedRoomCount);
                    walkers[i] = thisWalker;
                }
            }
            if(Random.Range(0f, 1f) < Scenario.GridComplexityIncreaseRate && walkers.Count < Scenario.GridComplexityLimit) SpawnWalker();
        }
    }
    private void FillContracts()
    {
        for(int i = 0; i < Scenario.ContractRooms.Length; i++)
        {
            contractedRooms.Add((byte)Random.Range(Scenario.ContractRooms[i].MinRoom - 1, Scenario.ContractRooms[i].MaxRoom + 1 - 1), Scenario.ContractRooms[i].SpawnerRoom);
        }
    }
    private Room SpawnRoom(Vector2 walkerPosition, int spawnedRoomIndex)
    {
        if(Scenario.SpawnRooms.Length == 0) throw new System.Exception("No rooms is specified.");
        byte[] roomChances = Scenario.SpawnRooms.Select(x => x.spawnRate).ToArray();
        Pay.Functions.Generic.BubbleSort(false, ref roomChances);
        byte min = roomChances[0];
        byte max = roomChances[roomChances.Length - 1];
        bool spawned = false;
        Vector2 spawnPosition = zeroRoomPos + walkerPosition  * Scenario.CellSize;
        while(spawned == false)
        {
            byte rotation = 0;
            if(contractedRooms.ContainsKey(spawnedRoomIndex))
            {
                contractedRooms.TryGetValue(spawnedRoomIndex, out SpawnerRoom room);
                if(room.availableRotations.Length != 0) rotation = room.availableRotations[Random.Range(0, room.availableRotations.Length)];
                contractedRooms.Remove(spawnedRoomIndex);
                
                
                
                return InstantiateRoom(room, rotation, spawnPosition , walkerPosition);
            }
            
            byte random = (byte)Random.Range(min, max + 1);
            byte index = (byte)Random.Range(0, Scenario.SpawnRooms.Length);
            if(Scenario.SpawnRooms[index].spawnRate >= random)
            {
                spawned = true;
                if(Scenario.SpawnRooms[index].SpawnerRoom.availableRotations.Length != 0) rotation = Scenario.SpawnRooms[index].SpawnerRoom.availableRotations[Random.Range(0, Scenario.SpawnRooms[index].SpawnerRoom.availableRotations.Length)];
                Room room = InstantiateRoom(Scenario.SpawnRooms[index].SpawnerRoom, rotation, spawnPosition, walkerPosition);
                room.spawner = this;
                return room;
            }
        }
        return null;
    }
    private Room InstantiateRoom(SpawnerRoom room, byte rotations, Vector2 spawnPosition, Vector2 walkerPosition)
    {
        Room obj = Instantiate(room.room, spawnPosition, Quaternion.identity);
        SpawnerRoom spawnerRoomInstance = room;
        spawnerRoomInstance.room = obj;
        Transform objTransform = obj.transform;
        objTransform.SetParent(RoomContainer);
        FilledCells.Add(walkerPosition, spawnerRoomInstance);
        objTransform.eulerAngles = objTransform.eulerAngles - Vector3.forward * rotations * 90f;
        DoorBehaviour[] doors = obj.GetComponentsInChildren<DoorBehaviour>();
        foreach(DoorBehaviour door in doors) door.SwapDirection(rotations);
        
        for(int i = 0; i < ActiveLevelRooms.Length; i++)
        {
            if(ActiveLevelRooms[i].Equals(default(SpawnerRoom))) 
            {
                ActiveLevelRooms[i] = spawnerRoomInstance;
                break;
            }
        }
        spawnedRoomCount++;
        return obj;
    }
    private void FillMark(SpawnerRoom room)
    {
        byte stageCount = (byte)Random.Range(room.minStageCount, room.maxStageCount + 1);
        room.room.MarkList = new ActionMark[stageCount];
        List<int> exclusiveStages = new List<int>();

        for(int i = 0; i < stageCount; i++)
        {
            System.Collections.Generic.List<MarkDatabase.MarkSlot> targetMarks = GetTargetRoomMarks(ref exclusiveStages, stageCount, (byte)i, room);
            
            while(targetMarks.Count != 0 && room.room.MarkList[i] == null)
            {
                int index = Random.Range(0, targetMarks.Count);
                if(targetMarks.Count == 1 || targetMarks[index].SpawnRate >= Random.Range(0, 1f))
                {
                    ActionMark mark = Instantiate(targetMarks[index].Mark);
                    room.room.MarkList[i] = mark;
                    break;
                }
                    
            }
        }
    }
    private System.Collections.Generic.List<MarkDatabase.MarkSlot> GetTargetRoomMarks(ref List<int> exclusiveStages, byte stageCount, byte currentStage, SpawnerRoom room)
    {
        System.Collections.Generic.List<MarkDatabase.MarkSlot> targetMarks = new List<MarkDatabase.MarkSlot>();
        if(stageCount != 0 && currentStage == stageCount - 1 && room.endMark.Length > 0) return room.endMark.ToList();
        else
        {
            for(int i = 0; i < room.stages.Length; i++)
            {
                if(room.stages[i].minStage <= currentStage + 1 && room.stages[i].maxStage >= currentStage + 1 && exclusiveStages.Contains(i) == false)
                {
                    targetMarks.AddRange(room.stages[i].marks);
                    if(room.stages[i].onlyOne == true) exclusiveStages.Add(i);
                }
            }
            return targetMarks;
        }
    }
    private void DoorsSetup()
    {
        for(int i = 0; i < FilledCells.Count; i++)
        {
            DoorBehaviour[] roomDoors = FilledCells.ElementAt(i).Value.room.GetComponentsInChildren<DoorBehaviour>().Where(x => x.ConnectedDoor == null).ToArray();
            foreach(DoorBehaviour currentDoor in roomDoors)
            {
                Vector2 direction = currentDoor.Direction;
                bool isRoomExist = FilledCells.TryGetValue(FilledCells.ElementAt(i).Key + direction, out SpawnerRoom wrappedRoom);
                if(isRoomExist)
                {
                    DoorBehaviour dependentDoor = wrappedRoom.room.GetComponentsInChildren<DoorBehaviour>().FirstOrDefault(x => x.ConnectedDoor == null && x.Direction + currentDoor.Direction == Vector2.zero);
                    
                    if(dependentDoor == null)
                    {
                        Destroy(currentDoor.gameObject);
                        continue;
                    }
                    Sprite dependentDoorSprite = dependentDoor.GetSprite();
                    Sprite currentDoorSprite = currentDoor.GetSprite();
                    dependentDoor.SetConnectedDoor(currentDoor, dependentDoorSprite);
                    currentDoor.SetConnectedDoor(dependentDoor, currentDoorSprite);
                }
                else Destroy(currentDoor.gameObject);
            }
        }
    }
    private void SetupObjects()
    {
        foreach(Transform gameObject in StartObjects) 
        {
            gameObject.position = FilledCells.ElementAt(0).Value.room.transform.position;
        }
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
        public byte minStageCount;
        public byte maxStageCount;
        public ScenarioStage[] stages;
        public MarkDatabase.MarkSlot[] endMark;
        public Room room;
        [Tooltip("1 turn = rotate 90 degrees clockwise"), Range(0, 3)] public byte[] availableRotations;
    }
    [System.Serializable]
    public struct ScenarioStage
    {
        public byte minStage;
        public byte maxStage;
        public bool onlyOne;
        public MarkDatabase.MarkSlot[] marks;
    }

    [System.Serializable]
    public struct RoomContract
    {
        public SpawnerRoom SpawnerRoom;
        public byte MinRoom;
        public byte MaxRoom;
    }
}