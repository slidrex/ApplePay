using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Charm Database")]
public class CharmDatabase : ItemAccessDatabase<int, CollectableCharm>
{
    [field: SerializeField] protected override ItemAccessDatabase<int, CollectableCharm>.DatabaseValue[] items {get; set;}
    public int GetLength() => items.Length;
}