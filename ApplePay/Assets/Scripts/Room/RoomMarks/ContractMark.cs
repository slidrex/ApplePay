using UnityEngine;

[CreateAssetMenu(menuName = "Marks/Room/New Contract Mark")]
public class ContractMark : ActionMark
{
    public float SpawnDelay;
    public byte MinContractObjects;
    public byte MaxContractObjects;
    public System.Collections.Generic.List<ContractObject> contractObjects = new System.Collections.Generic.List<ContractObject>();
    public override void ApplyMark(Room room) => StaticCoroutine.BeginCoroutine(SpawnContractObjects(room));
    private System.Collections.IEnumerator SpawnContractObjects(Room room)
    {
        Debug.Log("mark activate");
        yield return new WaitForSeconds(SpawnDelay);
        byte contractObjectCount = (byte)Random.Range(MinContractObjects, MaxContractObjects);
        for(int i = 0; i < contractObjectCount; i++)
        {
            bool instantiated = false;
            while(instantiated == false)
            {
                ContractObject curObject = contractObjects[Random.Range(0, contractObjects.Count)];
                if(curObject.SpawnChance >= Random.Range(0, 1f))
                {
                    Instantiate(curObject.Object, room.GetRandomFreeRoomSpace(), Quaternion.identity);
                    instantiated = true;
                    yield return new WaitForSecondsRealtime(Random.Range(curObject.MinSpawnInterval, curObject.MaxSpawnInterval));
                }
            }
        }
    }
    [System.Serializable]
    public struct ContractObject
    {
        public GameObject Object;
        [Range(0, 1f)] public float SpawnChance;
        public float MinSpawnInterval;
        public float MaxSpawnInterval;
    }
}
