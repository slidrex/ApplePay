using UnityEngine;

[CreateAssetMenu( menuName = "Databases/Mark database" )]

public class MarkDatabase : Database
{
    public MarkSlot[] MarkList;    
    [HideInInspector] public enum MarkType { None, Mob, Contract , Boss, Treasure, Bonus, End };
    [System.Serializable]
    public struct MarkSlot
    {
        public RoomMark Mark;
        public float SpawnChance;
    }
}
