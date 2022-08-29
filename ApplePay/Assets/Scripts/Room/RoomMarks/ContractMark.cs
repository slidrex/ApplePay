using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Marks/New Contract Mark")]
public class ContractMark : RoomMark
{
    public float SpawnDelay;
    public byte MinContractObjects;
    public byte MaxContractObjects;
    public List<ContractObject> contractObjects = new List<ContractObject>();
    [System.Serializable]
    public struct ContractObject
    {
        public GameObject Object;
        public float SpawnChance;
        public float MinSpawnInterval;
        public float MaxSpawnInterval;
    }
}
