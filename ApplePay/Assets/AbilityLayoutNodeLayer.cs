using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityLayoutNodeLayer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AbilityLayoutNode abilityNodeLayerCell;
    private AbilityLayoutNode[] cells;
    public AbilityNodeLayout AttachedLayout { get; private set; }
    [SerializeField] private UnityEngine.UI.Image mask;
    [SerializeField] private RectTransform layerContainer;
    private Vector2 screenSize;
    private RectTransform canvasRect;
    private void Start()
    {
        canvasRect = AttachedLayout.canvasRenderer.AttachedCanvas.GetComponent<RectTransform>();
    }
    public void SetActiveMask(bool active)
    {
        float alpha = active == true ? 0.1f : 0.0f;
        mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, alpha);
    }
    public void LinkLayout(AbilityNodeLayout layout)
    {
        AttachedLayout = layout;
    }
    private void OnEnable()
    {
        RescaleMask();
    }
    private void Update()
    {
        if(screenSize != canvasRect.sizeDelta) 
        {
            OnResize();
            screenSize = canvasRect.sizeDelta;
        }
    }
    private void OnResize()
    {
        RescaleMask();
    }
    private void RescaleMask()
    {
        StaticCoroutine.ExecuteOnEndOfFrame
        (
            () => mask.rectTransform.sizeDelta = new Vector2(layerContainer.sizeDelta.x, canvasRect.sizeDelta.y)/AttachedLayout.transform.localScale
        );
    }
    public void BuildLayer(Ability.NodeLayer layer)
    {
        if(cells != null)
            foreach(AbilityLayoutNode cell in cells)
                Destroy(cell.gameObject);
        
        int index = 0;
        cells = new AbilityLayoutNode[layer.Nodes.Length];
        foreach(AbilityNode node in layer.Nodes)
        {
            AbilityLayoutNode curNode = Instantiate(abilityNodeLayerCell, layerContainer);
            curNode.AttachLayer(this);
            RenderNode(curNode, node);
            cells[index] = curNode;
            index++;
        }
    }
    private void RenderNode(AbilityLayoutNode node, AbilityNode template)
    {
        
        if(template == null)
        {
            Color defaultNodeColor = Color.gray;
            RenderNode(node, "", defaultNodeColor, defaultNodeColor, defaultNodeColor);
        }
        else
        {
            LayoutNodeUtility.NodeColor rarityColor = LayoutNodeUtility.GetNodeColor(template.Display.Rarity);
            RenderNode(node, template.Display.Description.Name, rarityColor.backGroundColor, rarityColor.borderColor, rarityColor.textColor);
        }
    }
    private void RenderNode(AbilityLayoutNode node, string text, Color backGroundColor, Color borderColor, Color textColor)
    {
        node.SetBackgoundColor(backGroundColor);
        node.SetBorderColor(borderColor);
        node.SetNodeText(text, textColor);
    }


    public void OnNodePointerEnter(AbilityLayoutNode node)
    {

    }
    public void OnNodePointerExit(AbilityLayoutNode node)
    {
        
    }
    public void OnDrag(PointerEventData eventData) => AttachedLayout.OnLayerDrag(this);

    public void OnEndDrag(PointerEventData eventData) => AttachedLayout.OnLayerDragEnd(this);

    public void OnBeginDrag(PointerEventData eventData) => AttachedLayout.OnLayerDragBegin(this);
    public void OnPointerEnter(PointerEventData eventData) => AttachedLayout.OnLayerPointerEnter(this);

    public void OnPointerExit(PointerEventData eventData) => AttachedLayout.OnLayerPointerExit(this);

    private static class LayoutNodeUtility
    {
        internal struct NodeColor
        {
            public Color32 backGroundColor;
            public Color32 textColor;
            public Color32 borderColor;
            internal NodeColor(Color32 background, Color32 border)
            {
                backGroundColor = background;
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
