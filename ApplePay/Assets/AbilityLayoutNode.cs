using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityLayoutNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private Image border;
    [SerializeField] private Image background;
    [SerializeField] private Text nodeText;
    [SerializeField] private Vector2 nodeExpandPadding;
    [SerializeField] private Image mask;
    public AbilityLayoutNodeLayer AttachedLayer { get; internal set; }
    public RectTransform RectTransform { get; private set; }
    public AbilityNode RenderingNode { get; private set; }
    public int GetRenderNodeStackIndex() => RenderingNode.NodeStackIndex;
    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
    public void SetRaycastTargetFlag(bool isTarget)
    {
        nodeText.raycastTarget = isTarget;
        border.raycastTarget = isTarget;
        background.raycastTarget = isTarget;
        mask.raycastTarget = isTarget;
    }
    private void OnEnable()
    {
        StaticCoroutine.ExecuteOnEndOfFrame(SetRenderSize);
    }
    public void RenderNode(AbilityNode node)
    {
        if(node != null)
        {
            LayoutNodeUtility.NodeColor color = LayoutNodeUtility.GetNodeColor(node.Display.Rarity);
            SetBackgoundColor(color.backgroundColor);
            SetBorderColor(color.borderColor);
            SetNodeText(node.Display.Description.Name, color.textColor);
        }
        else
        {
            SetDefaultRendering();
        }
        RenderingNode = node;
    }
    private void SetDefaultRendering()
    {
        Color32 nullRenderColor = Color.gray;
        SetBackgoundColor(nullRenderColor);
        SetBorderColor(nullRenderColor);
        SetNodeText("", nullRenderColor);
    }
    private void SetMaskColor(Color color) => mask.color = color;
    public void SetActiveMask(bool active) => mask.gameObject.SetActive(active);
    private void SetBackgoundColor(Color color) => background.color = color;
    private void SetNodeText(string text, Color color)
    {
        char[] str = text.ToCharArray();
        for(int i = 0; i < str.Length; i++)
        {
            if(str[i] == ' ') str[i] = '\n';
        }
        nodeText.text = new string(str);
        nodeText.color = color;
    }
    private void SetBorderColor(Color color) => border.color = color;
    public void SetRenderSize()
    {
        Vector2 nodeBounds = background.rectTransform.rect.size * background.transform.localScale * (Vector2.one + nodeExpandPadding);
        Vector2 textSize = nodeText.rectTransform.rect.size * nodeText.transform.localScale;
        if(textSize.x > nodeBounds.x) background.rectTransform.sizeDelta = new Vector2(textSize.x * (1 + nodeExpandPadding.x), background.rectTransform.sizeDelta.y);
        if(textSize.y > nodeBounds.y) background.rectTransform.sizeDelta = new Vector2(background.rectTransform.sizeDelta.x, textSize.y * (1 + nodeExpandPadding.y));
    }

    public void OnBeginDrag(PointerEventData eventData) => AttachedLayer.OnNodeDragBegin(this);
    public void OnDrag(PointerEventData eventData) => AttachedLayer.OnNodeDrag(this);
    public void OnEndDrag(PointerEventData eventData) => AttachedLayer.OnNodeDragEnd(this);
    public void OnPointerEnter(PointerEventData eventData) => AttachedLayer.OnNodePointerEnter(this);
    public void OnPointerExit(PointerEventData eventData) => AttachedLayer.OnNodePointerExit(this);
    public void OnDrop(PointerEventData eventData) => AttachedLayer.AttachedLayout.OnNodeDraggingNodeTaken(this);
    private static class LayoutNodeUtility
    {
        internal struct NodeColor
        {
            public Color32 backgroundColor;
            public Color32 textColor;
            public Color32 borderColor;
            internal NodeColor(Color32 background, Color32 border)
            {
                backgroundColor = background;
                textColor = border;
                borderColor = border;
            }
        }
        private static System.Collections.Generic.Dictionary<ItemRarity, NodeColor> Colors = new System.Collections.Generic.Dictionary<ItemRarity, NodeColor>
        {
            [ItemRarity.Ordinary] = new NodeColor(new Color32(85, 105, 85, 255), new Color32(218, 255, 218, 255)),
            [ItemRarity.ExtraOrdinary] = new NodeColor(new Color32(65, 0, 65, 255), new Color32(255, 138, 255, 255)),
            [ItemRarity.Mythical] = new NodeColor(new Color32(40, 40, 20, 255), new Color32(255, 236, 118, 255)),
        };
        public static NodeColor GetNodeColor(ItemRarity rarity)
        {
            return Colors[rarity];
        }
    }
}
