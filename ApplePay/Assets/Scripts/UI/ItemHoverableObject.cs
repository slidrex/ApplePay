using UnityEngine;

public class ItemHoverableObject : MonoBehaviour
{
    public ItemDescription Description;
    [SerializeField] private DroppedItemContentPanel contentPanel;
    [SerializeField] protected string header;
    [SerializeField, TextArea] protected string description;
    private Pay.UI.UIHolder holder;
    private DroppedItemContentPanel tempPanel;
    private Vector2 sourceOffset = Vector2.up;
    private bool onMouse;
    private void Start() => holder = FindObjectOfType<Pay.UI.UIHolder>();
    private void Update() 
    {
        PositionUpdate();
        if(onMouse) OnMouse();
    }
    private void OnMouse()
    {
        if(tempPanel == null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            CreatePanel();
        
        if(tempPanel != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            TerminatePanel();
    }
    private void OnMouseEnter()
    {
        onMouse = true;
        if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            CreatePanel();
    }
    private void OnMouseExit()
    {
        onMouse = false;
        TerminatePanel();
    }
    private void CreatePanel() 
    {
        tempPanel = Instantiate(contentPanel.gameObject, (Vector2)transform.position + sourceOffset, Quaternion.identity, holder.FollowCanvas.transform)
                .GetComponent<DroppedItemContentPanel>();
        tempPanel.UpdateContentPanel();
        tempPanel.SetHeader(header);
        tempPanel.SetDescription(description);
    }
    public void TerminatePanel() 
    {
        if(tempPanel != null && tempPanel.GetAnimator() != null) tempPanel.GetAnimator().SetTrigger("CloseImage");
    }
    private void PositionUpdate()
    {
        Vector2 sourcePosition = (Vector2)transform.position + sourceOffset;
        if(tempPanel != null && (Vector2)tempPanel.transform.position != sourcePosition)
            tempPanel.transform.position = sourcePosition;
    }
    public DroppedItemContentPanel GetCurrentPanel() => tempPanel;
}
