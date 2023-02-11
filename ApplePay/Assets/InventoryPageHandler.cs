using UnityEngine;

public class InventoryPageHandler : InventoryElement
{
    public UnityEngine.Canvas Canvas;
    public UnityEngine.UI.CanvasScaler CanvasScaler;
    public InventoryUIPage[] Pages;
    [SerializeField] private KeyCode previousRepositoryKey;
    [SerializeField] private KeyCode nextRepositoryKey;
    public int spacing;
    public float animationSpeed;
    public int CurrentRepository { get; set; }
    private bool isSwitching;
    public InventorySwitchAnimation currentAnimation;
    private void Awake()
    {
        foreach(InventoryUIPage page in Pages)
        {
            page.AttachCanvas(Canvas);
        }
    }
    public void ActiveInventoryPage(int index)
    {
        MenuHandler.DeactivateMenuComponents();
        gameObject.SetActive(true);
        foreach(InventoryUIPage element in Pages)
        {
            element.gameObject.gameObject.SetActive(false);
        }
        Pages[index].transform.gameObject.SetActive(true);
        CurrentRepository = index;   
    }
    private void Update()
    {
        if(isSwitching) OnSwitchUpdate();
        else
        {
            if(Input.GetKeyDown(nextRepositoryKey) && Pages.Length > 1)
            {
                SwitchRepository(true);
            }
            if(Input.GetKeyDown(previousRepositoryKey) && Pages.Length > 1)
            {
                SwitchRepository(false);
            }
        }
    }
    private void OnSwitchStart(int previousIndex, int newIndex, bool next)
    {
        int offsetDirection = next ? 1 : -1;
        Vector2 newStartPosition = new Vector2((CanvasScaler.referencePixelsPerUnit + spacing) * offsetDirection, 0.0f);
        Pages[newIndex].gameObject.SetActive(true);
        Pages[newIndex].transform.localPosition = newStartPosition;
        isSwitching = true;
        currentAnimation = new InventorySwitchAnimation();
        currentAnimation.swappingRepository = Pages[previousIndex].Transform;
        currentAnimation.targetRepository = Pages[newIndex].Transform;
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
        int previousIndex = CurrentRepository;
        CurrentRepository = (int)UnityEngine.Mathf.Repeat(CurrentRepository + offset, Pages.Length);
        
        OnSwitchStart(previousIndex, CurrentRepository, right);
    }
    public struct InventorySwitchAnimation
    {
        public UnityEngine.RectTransform targetRepository;
        public UnityEngine.RectTransform swappingRepository;
        public Vector2 swappingRepositoryEndPosition;
    }
}
