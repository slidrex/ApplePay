using UnityEngine;

public class PlayerMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] ToggleMenuItems;
    [SerializeField] private Transform menuComponents;
    [SerializeField] private Transform repositoryList;
    [SerializeField] private Transform repositoryContainer;
    [SerializeField, Tooltip("objects that active while the menu is open")] private GameObject[] internalElements;
    public bool IsOpen;
    [SerializeField] private KeyCode menuActivateKey;
    private Creature owner;
    private byte constraintID;
    private Creature.EntityState engagingState;
    public InventoryMenuState MenuState = InventoryMenuState.None;
    public enum InventoryMenuState
    {
        None,
        RepositoryList,
        InRepository
    }
    private void Start()
    {
        owner = GetComponent<Creature>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(menuActivateKey))
        {
            if(IsOpen == false && owner.IsFree() == false) return;
            
            ToggleMenu();

        }
    }
    private void ToggleMenu()
    {
        IsOpen = !IsOpen;
        menuComponents.gameObject.SetActive(IsOpen);
        DeactivateMenuComponents();
        SetMenuElementsActive(IsOpen, internalElements);
        SetMenuElementsActive(IsOpen, ToggleMenuItems);
        GetComponent<Animator>().SetBool("isMoving", false);

        if(IsOpen) OnMenuOpen();
        else OnMenuClose();
    }
    private void OnMenuOpen()
    {
        PayWorld.EffectController.AddEffect(owner, out constraintID, new PayWorld.Effect.EffectProperty(PayWorld.Effect.EffectActionPresets.MoveConstraint()));
        engagingState = owner.Engage(null);
    }
    private void OnMenuClose()
    {
        PayWorld.EffectController.RemoveEffect(owner, ref constraintID);
        engagingState.Remove();
    }
    public void SetMenuElementsActive(bool isActive, GameObject[] objects)
    {
        foreach(GameObject gameObject in objects)
        {
            gameObject.SetActive(isActive);
        }
    }
    public void DeactivateMenuComponents()
    {
        DeactivateChildren(menuComponents);
        foreach(GameObject gameObject in internalElements)
        {
            gameObject.SetActive(true);
        }
        MenuState = InventoryMenuState.None;
    }
    ///<summary>Deactivates all transform children.</summary>
    public void DeactivateChildren(Transform transform)
    {
        foreach(Transform child in transform) 
        {
            child.gameObject.SetActive(false);
        }
    }
    public void OpenRepositoryList()
    {
        DeactivateMenuComponents();
        MenuState = InventoryMenuState.RepositoryList;
        repositoryList.gameObject.SetActive(true);
    }
    public void OpenRepository(GameObject repository)
    {
        DeactivateMenuComponents();
        DeactivateChildren(repositoryContainer);
        repositoryContainer.gameObject.SetActive(true);
        repository.gameObject.SetActive(true);
        MenuState = InventoryMenuState.InRepository;
    }
}