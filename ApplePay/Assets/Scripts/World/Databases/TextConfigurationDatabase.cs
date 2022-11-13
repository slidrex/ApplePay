using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text Configuration database")]
public class TextConfigurationDatabase : ItemAccessDatabase<string, Pay.UI.TextConfiguration>
{
    [field: SerializeField] protected override DatabaseValue[] items { get; set; }
}