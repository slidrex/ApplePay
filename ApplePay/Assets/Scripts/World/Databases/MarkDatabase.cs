using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( menuName = "Databases/Mark database" )]
public class MarkDatabase : ScriptableObject
{
    public List<MarkSlot> MarkList = new List<MarkSlot>();    
    [HideInInspector] public enum MarkType { None, Mob, Contract , Boss, Treasure, Bonus, End };
    [System.Serializable]
    public struct MarkSlot
    {
        public RoomMark Mark;
        public float SpawnChance;
    }
}
