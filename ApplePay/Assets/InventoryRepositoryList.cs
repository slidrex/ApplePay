public class InventoryRepositoryList : InventoryElement
{
    public void Activate()
    {
        MenuHandler.DeactivateMenuComponents();
        gameObject.SetActive(true);
    }
}
