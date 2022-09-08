using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text Configuration database")]
public class TextConfigurationDatabase : Database
{
    [SerializeField] private TextConfigurationDatabaseItem[] Items;
    public Pay.UI.TextConfiguration Find(string id)
    {
        foreach(TextConfigurationDatabaseItem item in Items)
        {
            if(item.Name.Equals(id)) return item.Configuration;
        }
        Debug.LogWarning("Configuration with id " + id + " doesn't exist.");
        return null;
    }
}
[System.Serializable]
public struct TextConfigurationDatabaseItem
{
    public string Name;
    public Pay.UI.TextConfiguration Configuration;
}
