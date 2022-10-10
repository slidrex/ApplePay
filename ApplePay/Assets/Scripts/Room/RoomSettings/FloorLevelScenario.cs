using UnityEngine;

[CreateAssetMenu(menuName = "Floor Level Scenario")]

public class FloorLevelScenario : ScriptableObject
{
    public RoomSpawner.RandRoom[] SpawnRooms;
    public int RoomCount = 1;
    [Header("Grid generation settings")]
    public float CellSize = 50f;
    public byte StartGridComplexity = 1, GridComplexityLimit = 1;
    [Range(0, 1f)] public float GridComplexityIncreaseRate;
    [Range(0, 1)] public float ChangeDirectionRate;
    [Header("Contracts")]
    public RoomSpawner.RoomContract[] ContractRooms;
}
