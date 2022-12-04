using UnityEngine;

[System.Serializable]
public class ItemHoverableObject
{
    [SerializeField] private DroppedItemContentPanel contentPanel;
    private Pay.UI.UIHolder holder;
    private DroppedItemContentPanel tempPanel;
    private Vector2 sourceOffset = Vector2.up;
    private Transform sourceTransform;
    private bool onMouse;
    public bool Initiated { get; set; }
    public void Init(Transform sourceTransform) 
    {
        holder = MonoBehaviour.FindObjectOfType<Pay.UI.UIHolder>();
        this.sourceTransform = sourceTransform;
        Initiated = true;
    }
    public void Update(string header, string description) 
    {
        PositionUpdate(sourceTransform);
        if(onMouse) OnMouse(header, description);
    }
    private void OnMouse(string header, string description)
    {
        if(tempPanel == null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            CreatePanel(sourceTransform, header, description);
        
        if(tempPanel != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            TerminatePanel();
    }
    public void OnMouseEnter(string header, string description)
    {
        onMouse = true;
        if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            CreatePanel(sourceTransform, header, description);
    }
    public void OnMouseExit()
    {
        onMouse = false;
        TerminatePanel();
    }
    private void CreatePanel(Transform transform, string header, string description) 
    {
        tempPanel = MonoBehaviour.Instantiate(contentPanel.gameObject, (Vector2)transform.position + sourceOffset, Quaternion.identity, holder.FollowCanvas.transform)
                .GetComponent<DroppedItemContentPanel>();
        tempPanel.UpdateContentPanel();
        tempPanel.SetHeader(header);
        tempPanel.SetDescription(description);
    }
    public void TerminatePanel() 
    {
        if(tempPanel != null && tempPanel.GetAnimator() != null) tempPanel.GetAnimator().SetTrigger("CloseImage");
    }
    private void PositionUpdate(Transform transform)
    {
        Vector2 sourcePosition = (Vector2)transform.position + sourceOffset;
        if(tempPanel != null && (Vector2)tempPanel.transform.position != sourcePosition)
            tempPanel.transform.position = sourcePosition;
    }
    public DroppedItemContentPanel GetCurrentPanel() => tempPanel;
    public void OnDestroy()
    {
        TerminatePanel();
    }
}
