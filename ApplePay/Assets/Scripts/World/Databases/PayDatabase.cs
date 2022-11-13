public class PayDatabase : UnityEngine.MonoBehaviour
{
    
    private const string DatabaseObjectPath = "Databases/Database object";
    public static Database GetDatabase(string databaseID)
    {
        DatabaseObject databaseObject = UnityEngine.Resources.Load<DatabaseObject>(DatabaseObjectPath);
        if(databaseObject == null) throw new System.Exception("Path (" + "Resources/" + DatabaseObjectPath + ") doesn't contain Database object.");
        return databaseObject.GetItem(databaseID);
    }
    
}