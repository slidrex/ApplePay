using UnityEngine;

public class InventoryRepositoryHandler : InventoryElement
{
    public UnityEngine.UI.CanvasScaler Canvas;
    public InventoryRepositoryElement[] Repositories;
    [SerializeField] private KeyCode previousRepositoryKey;
    [SerializeField] private KeyCode nextRepositoryKey;
    public int spacing;
    public float animationSpeed;
    public int currentRepository;
    private bool isSwitching;
    public InventorySwitchAnimation currentAnimation;
    public void ActiveRepository(RepositoryRendererBase repositoryRenderer)
    {
        MenuHandler.DeactivateMenuComponents();
        gameObject.SetActive(true);
        foreach(InventoryRepositoryElement element in Repositories)
        {
            element.repositoryRenderer.gameObject.SetActive(false);
        }
        for(int i = 0; i < Repositories.Length; i++)
        {
            if(Repositories[i].repositoryRenderer == repositoryRenderer)
            {
                repositoryRenderer.gameObject.SetActive(true);
                currentRepository = i;   
            }

        }
    }
    private void Update()
    {
        if(isSwitching) OnSwitchUpdate();
        else
        {
            if(Input.GetKeyDown(nextRepositoryKey))
            {
                SwitchRepository(true);
            }
            if(Input.GetKeyDown(previousRepositoryKey))
            {
                SwitchRepository(false);
            }
        }
    }
    private void OnSwitchStart(int previousIndex, int newIndex, bool next)
    {
        int offsetDirection = next ? 1 : -1;
        Vector2 newStartPosition = new Vector2((Canvas.referenceResolution.x + spacing) * offsetDirection, 0.0f);
        Repositories[newIndex].repositoryRenderer.gameObject.SetActive(true);
        Repositories[newIndex].transform.localPosition = newStartPosition;
        isSwitching = true;
        currentAnimation = new InventorySwitchAnimation();
        currentAnimation.swappingRepository = Repositories[previousIndex].transform;
        currentAnimation.targetRepository = Repositories[newIndex].transform;
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
    [System.Serializable]
    public struct InventoryRepositoryElement
    {
        public RepositoryRendererBase repositoryRenderer;
        public UnityEngine.RectTransform transform; 
    }
    public struct InventorySwitchAnimation
    {
        public UnityEngine.RectTransform targetRepository;
        public UnityEngine.RectTransform swappingRepository;
        public Vector2 swappingRepositoryEndPosition;
    }
}
