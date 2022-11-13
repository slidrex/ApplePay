using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Sprite Database")]
public class SpriteDatabase : ItemAccessDatabase<string, Sprite>
{
    [field: SerializeField] protected override ItemAccessDatabase<string, Sprite>.DatabaseValue[] items { get; set; }
}
