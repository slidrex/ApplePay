using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Database object")]
public class DatabaseObject : ScriptableObject
{
    public DatabaseObjectItem[] Databases;
    public Database Find(string id)
    {
        foreach(DatabaseObjectItem item in Databases)
        {
            if(item.Id.Equals(id)) return item.Database;
        }
        Debug.LogWarning("Database with id " + id + " doesn't exist.");
        return null;
    }
}
[System.Serializable]
public struct DatabaseObjectItem
{
    public string Id;
    public Database Database;
}
