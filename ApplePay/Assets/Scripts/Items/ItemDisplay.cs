[System.Serializable]
public class ItemDisplay
{
    public UnityEngine.Sprite InventorySprite;
}
public class WeaponDisplay : ItemDisplay
{
    public ItemDescription Description;
}
[System.Serializable]
public class ItemDescription
{
    public string Name;
    public string Description;

}