using UnityEngine;

[CreateAssetMenu( menuName = "Databases/Mark database" )]

public class MarkDatabase : Database
{
    public MarkSlot[] MarkList;    
    [System.Serializable]
    public struct MarkSlot
    {
        public ActionMark Mark;
        [Range(0, 1f)] public float SpawnChance;
    }
}