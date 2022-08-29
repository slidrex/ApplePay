using UnityEngine;
public class ItemHoverableObject : MonoBehaviour
{
    [SerializeField] private DroppedItemContentPanel contentPanel;
    [SerializeField] private string header;
    [SerializeField, TextArea] private string description;
    private Pay.UI.UIHolder holder;
    private DroppedItemContentPanel tempPanel;
    private Vector2 sourceOffset = Vector2.up;
    private void Start() => holder = FindObjectOfType<Pay.UI.UIHolder>();
    private void Update() => PositionUpdate();
    private void OnMouseEnter()
    {
        tempPanel = Instantiate(contentPanel.gameObject, (Vector2)transform.position + sourceOffset, Quaternion.identity, holder.FollowCanvas.transform)
            .GetComponent<DroppedItemContentPanel>();
        tempPanel.UpdateContentPanel();
        tempPanel.SetHeader(header);
        tempPanel.SetDescription(description);
    }
    private void OnMouseExit()
    {
        Destroy(tempPanel.gameObject);
    }
    private void PositionUpdate()
    {
        Vector2 sourcePosition = (Vector2)transform.position + sourceOffset;
        if(tempPanel != null && (Vector2)tempPanel.transform.position != sourcePosition)
            tempPanel.transform.position = sourcePosition;
    }
    public DroppedItemContentPanel GetCurrentPanel() => tempPanel;
}
