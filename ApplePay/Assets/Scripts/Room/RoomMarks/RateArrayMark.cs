[UnityEngine.CreateAssetMenu(menuName = "Marks/Array/New Rate Array Mark")]

public class RateArrayMark : RoomMark
{
    public RateMarkObject[] Items;

    [System.Serializable]
    public struct RateMarkObject
    {
        public UnityEngine.GameObject item;
        [UnityEngine.Range(0, 100)] public int rate;
    }
}
