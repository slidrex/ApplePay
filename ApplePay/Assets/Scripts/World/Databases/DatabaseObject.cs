using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Database object")]
public class DatabaseObject : ItemAccessDatabase<string, Database>
{
    [field: SerializeField] protected override DatabaseValue[] items { get; set; }
}