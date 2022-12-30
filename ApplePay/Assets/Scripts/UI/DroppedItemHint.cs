using UnityEngine;

[System.Serializable]
public class DroppedItemHint
{
    [SerializeField] private DroppedItemContentPanel contentPanel;
    private DroppedItemContentPanel tempPanel;
    private Vector2 sourceOffset = Vector2.up;
    private Transform sourceTransform;
    public bool HintCreated;
    public void Init(Transform sourceTransform) 
    {
        this.sourceTransform = sourceTransform;
    }
    public void CreateHint(string header, string description)
    {
        HintCreated = true;
        tempPanel = MonoBehaviour.Instantiate(contentPanel, (Vector2)sourceTransform.position + sourceOffset, Quaternion.identity);
        tempPanel.UpdateContentPanel();
        SetPosition();
        tempPanel.SetHeader(header);
        tempPanel.SetDescription(description);
    }
    public void DestroyHint()
    {
        HintCreated = false;
        
        tempPanel.GetAnimator().SetTrigger("CloseImage");
    }
    private void SetPosition()
    {
        Vector2 sourcePosition = (Vector2)sourceTransform.position + sourceOffset;
        
        tempPanel.transform.position = sourcePosition;
    }
}
