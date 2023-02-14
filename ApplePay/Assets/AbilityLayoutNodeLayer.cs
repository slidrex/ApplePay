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
    public Ability.NodeLayer RenderingLayer { get; private set; }
    public int GetRenderLayerStackIndex() => RenderingLayer.LayerStackIndex;
    private void Start()
    {
        canvasRect = AttachedLayout.canvasRenderer.AttachedCanvas.GetComponent<RectTransform>();
    }
    public void SetActiveMask(bool active) => mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, active == true ? 0.1f : 0.0f);
    private void OnEnable() => RescaleMask();
    private void Update()
    {
        if(screenSize != canvasRect.sizeDelta) 
        {
            OnResize();
            screenSize = canvasRect.sizeDelta;
        }
    }
    private void OnResize() => RescaleMask();
    private void RescaleMask()
    {
        StaticCoroutine.ExecuteOnEndOfFrame
        (
            () => mask.rectTransform.sizeDelta = new Vector2(layerContainer.sizeDelta.x, canvasRect.sizeDelta.y)/AttachedLayout.transform.localScale
        );
    }
    public void BuildLayer(AbilityNodeLayout layout, Ability.NodeLayer layer)
    {
        RenderingLayer = layer;
        AttachedLayout = layout;
        if(cells != null)
            foreach(AbilityLayoutNode cell in cells)
                Destroy(cell.gameObject);
        
        int index = 0;
        cells = new AbilityLayoutNode[layer.Nodes.Length];
        foreach(AbilityNode node in layer.Nodes)
        {
            AbilityLayoutNode curNode = Instantiate(abilityNodeLayerCell, layerContainer);
            curNode.AttachedLayer = this;
            curNode.RenderNode(node);
            cells[index] = curNode;
            index++;
        }
    }
    public void OnNodePointerEnter(AbilityLayoutNode node) => AttachedLayout.OnNodePointerEnter(node);
    public void OnNodePointerExit(AbilityLayoutNode node) => AttachedLayout.OnNodePointerExit(node);
    public void OnNodeDragBegin(AbilityLayoutNode node) => AttachedLayout.OnNodeDragBegin(node);
    public void OnNodeDrag(AbilityLayoutNode node) => AttachedLayout.OnNodeDrag(node);
    public void OnNodeDragEnd(AbilityLayoutNode node) => AttachedLayout.OnNodeDragEnd(node);
    public void OnDrag(PointerEventData eventData) => AttachedLayout.OnLayerDrag(this);
    public void OnEndDrag(PointerEventData eventData) => AttachedLayout.OnLayerDragEnd(this);
    public void OnBeginDrag(PointerEventData eventData) => AttachedLayout.OnLayerDragBegin(this);
    public void OnPointerEnter(PointerEventData eventData) => AttachedLayout.OnLayerPointerEnter(this);
    public void OnPointerExit(PointerEventData eventData) => AttachedLayout.OnLayerPointerExit(this);
}
