public static class PayDatabase
{
    private const string DatabaseObjectPath = "Assets/Databases/Database object.asset";
    public static Database GetDatabase(string databaseID)
    {
        DatabaseObject databaseObject = (DatabaseObject)UnityEditor.AssetDatabase.LoadAssetAtPath(DatabaseObjectPath, typeof(DatabaseObject));
        if(databaseObject == null) throw new System.Exception("Path (" + DatabaseObjectPath + ") doesn't contain Database object.");
        return databaseObject.Find(databaseID);
    }
}