using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityLayoutNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image border;
    [SerializeField] private Image background;
    [SerializeField] private Text nodeText;
    [SerializeField] private Vector2 nodeExpandPadding;
    [SerializeField] private Image mask;
    private AbilityLayoutNodeLayer layer;
    private void OnEnable()
    {
        StaticCoroutine.ExecuteOnEndOfFrame(ResizeNode);
    }
    public void AttachLayer(AbilityLayoutNodeLayer layer)
    {
        this.layer = layer;
    }
    public void SetMaskColor(Color color)
    {
        mask.color = color;
    }
    public void SetActiveMask(bool active)
    {
        mask.gameObject.SetActive(active);
    }
    public void SetBackgoundColor(Color color)
    {
        background.color = color;
    }
    public void SetNodeText(string text, Color color)
    {
        char[] str = text.ToCharArray();
        for(int i = 0; i < str.Length; i++)
        {
            if(str[i] == ' ') str[i] = '\n';
        }
        nodeText.text = new string(str);
        nodeText.color = color;
    }
    public void SetBorderColor(Color color)
    {
        border.color = color;
    }
    private void ResizeNode()
    {
        Vector2 nodeBounds = background.rectTransform.rect.size * background.transform.localScale * (Vector2.one + nodeExpandPadding);
        Vector2 textSize = nodeText.rectTransform.rect.size * nodeText.transform.localScale;
        if(textSize.x > nodeBounds.x) background.rectTransform.sizeDelta = new Vector2(textSize.x * (1 + nodeExpandPadding.x), background.rectTransform.sizeDelta.y);
        if(textSize.y > nodeBounds.y) background.rectTransform.sizeDelta = new Vector2(background.rectTransform.sizeDelta.x, textSize.y * (1 + nodeExpandPadding.y));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData) => layer.AttachedLayout.OnNodePointerEnter(this);

    public void OnPointerExit(PointerEventData eventData) => layer.AttachedLayout.OnNodePointerExit(this);
}
