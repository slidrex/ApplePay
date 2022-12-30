using UnityEngine;

public class InventoryRepositoryHandler : InventoryElement
{
    public UnityEngine.UI.CanvasScaler Canvas;
    public InventoryUIPage[] Repositories;
    [SerializeField] private KeyCode previousRepositoryKey;
    [SerializeField] private KeyCode nextRepositoryKey;
    public int spacing;
    public float animationSpeed;
    public int currentRepository;
    private bool isSwitching;
    public InventorySwitchAnimation currentAnimation;
    public void ActiveRepository(int index)
    {
        MenuHandler.DeactivateMenuComponents();
        gameObject.SetActive(true);
        foreach(InventoryUIPage element in Repositories)
        {
            element.gameObject.gameObject.SetActive(false);
        }
        Repositories[index].transform.gameObject.SetActive(true);
        currentRepository = index;   
    }
    private void Update()
    {
        if(isSwitching) OnSwitchUpdate();
        else
        {
            if(Input.GetKeyDown(nextRepositoryKey) && Repositories.Length > 1)
            {
                SwitchRepository(true);
            }
            if(Input.GetKeyDown(previousRepositoryKey) && Repositories.Length > 1)
            {
                SwitchRepository(false);
            }
        }
    }
    private void OnSwitchStart(int previousIndex, int newIndex, bool next)
    {
        int offsetDirection = next ? 1 : -1;
        Vector2 newStartPosition = new Vector2((Canvas.referenceResolution.x + spacing) * offsetDirection, 0.0f);
        Repositories[newIndex].gameObject.SetActive(true);
        Repositories[newIndex].transform.localPosition = newStartPosition;
        isSwitching = true;
        currentAnimation = new InventorySwitchAnimation();
        currentAnimation.swappingRepository = Repositories[previousIndex].Transform;
        currentAnimation.targetRepository = Repositories[newIndex].Transform;
        currentAnimation.swappingRepositoryEndPosition = -newStartPosition;
    }
    private void OnSwitchUpdate()
    {
        currentAnimation.targetRepository.localPosition = Vector3.MoveTowards(currentAnimation.targetRepository.localPosition, Vector3.zero, animationSpeed);
        currentAnimation.swappingRepository.localPosition = Vector3.MoveTowards(currentAnimation.swappingRepository.localPosition, currentAnimation.swappingRepositoryEndPosition, animationSpeed);
        if(currentAnimation.targetRepository.localPosition == Vector3.zero)
        {
            OnSwitchEnd();
        }
    }
    private void OnSwitchEnd()
    {
        isSwitching = false;
        currentAnimation.swappingRepository.gameObject.SetActive(false);
        currentAnimation = default(InventorySwitchAnimation);
    }

    public void SwitchRepository(bool right)
    {
        int offset = right? 1 : -1;
        int previousIndex = currentRepository;
        currentRepository = (int)UnityEngine.Mathf.Repeat(currentRepository + offset, Repositories.Length);
        
        OnSwitchStart(previousIndex, currentRepository, right);
    }
    public struct InventorySwitchAnimation
    {
        public UnityEngine.RectTransform targetRepository;
        public UnityEngine.RectTransform swappingRepository;
        public Vector2 swappingRepositoryEndPosition;
    }
}
