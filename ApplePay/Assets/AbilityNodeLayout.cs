using UnityEngine;

public class AbilityNodeLayout : MonoBehaviour
{
    [SerializeField] private AbilityLayoutNodeLayer _layer;
    private AbilityLayoutNodeLayer[] layoutLayers;
    private float leftConfinerX;
    private float rightConfinerX;
    private RectTransform rectTransform;
    public Ability AttachedAbility { get; private set; }
    [SerializeField] private NodeHoverboard nodeHoverboard;
    private NodeHoverboard activatedHoverboard;
    public AbilityCanvasRenderer canvasRenderer { get; private set; }
    [SerializeField] private float hoverboardOffset;
    private LayoutState currentState;
    private Vector2[] layerPositionsBeforeDrag;
    private enum LayoutState
    {
        Free,
        MovingLayer,
        MovingNode
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void AttachRenderer(AbilityCanvasRenderer renderer)
    {
        canvasRenderer = renderer;
    }
    public void BuildLayout(Ability ability)
    {
        AttachedAbility = ability;
        if(layoutLayers != null)
            foreach(AbilityLayoutNodeLayer currentLayer in layoutLayers)
                Destroy(currentLayer);
        
        Ability.NodeLayer[] nodeLayerInstance = ability.GetLayersInstance(); 
        
        layoutLayers = new AbilityLayoutNodeLayer[nodeLayerInstance.Length];
        for(int i = 0; i < layoutLayers.Length; i++)
        {
            AbilityLayoutNodeLayer curLayer = Instantiate(_layer, transform);
            curLayer.BuildLayer(this, nodeLayerInstance[i]);
            layoutLayers[i] = curLayer;
        }
    }
    public void OnLayerPointerEnter(AbilityLayoutNodeLayer layer)
    {
        layer.SetActiveMask(true);
    }
    public void OnLayerPointerExit(AbilityLayoutNodeLayer layer)
    {
        layer.SetActiveMask(false);
    }
    private NodeHoverboard CreateNodeHoverboard(AbilityLayoutNode node)
    {
        NodeHoverboard activatedHoverboard = Instantiate(nodeHoverboard, node.transform);
        AbilityNode currentNode = node.RenderingNode;
        activatedHoverboard.SetDescription(currentNode.Display.Description.Description);
        activatedHoverboard.SetEnergyCost(currentNode.GetNodeEnergyCost());
        activatedHoverboard.SetCooldown((int)currentNode.GetNodeCooldown());
        activatedHoverboard.SetName(currentNode.Display.Description.Name);
        if(currentNode.NodeTime != 0.0f)
        {
            activatedHoverboard.AddStatField("Duration", currentNode.NodeTime, false);
        }
        foreach(AbilityNode.NodeField field in currentNode.GetFields())
        {
            activatedHoverboard.AddStatField(field.FieldName, field.FieldScore * field.ScoreMultiplier, field.ShowSign, field.NumberType == Pay.Functions.Math.NumberType.Unit ? false : true);
        }
        activatedHoverboard.transform.localPosition = Vector3.up * (node.RectTransform.sizeDelta.y * 0.5f + hoverboardOffset);
        return activatedHoverboard;
    }
    private AbilityLayoutNode instanceNode;
    private AbilityLayoutNode draggingNode;
    private AbilityNode draggingn;
    private int draggingNodeStackIndex;
    public void OnNodeDragBegin(AbilityLayoutNode node)
    {
        currentState = LayoutState.MovingNode;
        instanceNode = Instantiate(node, transform);
        draggingNode = node;
        draggingn = draggingNode.RenderingNode;
        draggingNodeStackIndex = node.RenderingNode.NodeStackIndex;
        node.RenderNode(null);
        instanceNode.SetRaycastTargetFlag(false);
    }
    public void OnNodeDrag(AbilityLayoutNode node)
    {
        instanceNode.transform.position = (Vector3)Pay.Functions.Generic.GetMousePos(Camera.main);
    }
    public void OnNodeDragEnd(AbilityLayoutNode node)
    {
        currentState = LayoutState.Free;
        Destroy(instanceNode.gameObject);
    }
    public void OnNodeDraggingNodeTaken(AbilityLayoutNode takenLayoutNode)
    {
        AbilityLayoutNodeLayer takenNodeLayer = takenLayoutNode.AttachedLayer;
        AbilityLayoutNodeLayer draggingNodeLayer = draggingNode.AttachedLayer;

        AbilityNode takenNode = takenLayoutNode?.RenderingNode;
        AbilityNode _draggingNode = draggingn;
        
        
        AttachedAbility.SetNodeAt(draggingNodeLayer.GetRenderLayerStackIndex(), draggingNodeStackIndex, takenNode);
        if(takenNode != null)
        AttachedAbility.SetNodeAt(takenNodeLayer.GetRenderLayerStackIndex(), takenLayoutNode.GetRenderNodeStackIndex(), _draggingNode);

        draggingNode.RenderNode(takenNode);
        takenLayoutNode.RenderNode(_draggingNode);
        StaticCoroutine.ExecuteOnEndOfFrame(() => draggingNode.SetRenderSize());
        StaticCoroutine.ExecuteOnEndOfFrame(() => takenLayoutNode.SetRenderSize());
        Destroy(instanceNode);
    }
    public void OnNodePointerEnter(AbilityLayoutNode node)
    {
        if(currentState != LayoutState.MovingLayer)
            node.SetActiveMask(true);
        if(currentState == LayoutState.Free)
        {
            if(node.RenderingNode != null)
                activatedHoverboard = CreateNodeHoverboard(node);
        }
    }
    public void OnNodePointerExit(AbilityLayoutNode node)
    {
        if(activatedHoverboard != null) DestroyNodeHoverboard();
        node.SetActiveMask(false);
    }
    private void DestroyNodeHoverboard() => Destroy(activatedHoverboard.gameObject);
    public void OnLayerDragBegin(AbilityLayoutNodeLayer layer)
    {
        currentState = LayoutState.MovingLayer;

        layerPositionsBeforeDrag = new Vector2[layoutLayers.Length];
        for(int i = 0; i < layoutLayers.Length; i++) 
        {
            layerPositionsBeforeDrag[i] = layoutLayers[i].transform.position;
        }
        Vector2 bounds = GetBounds();
        rightConfinerX = bounds.y;
        leftConfinerX = bounds.x;
    }
    public void OnLayerDrag(AbilityLayoutNodeLayer layer)
    {
        layer.transform.position = new Vector3(Mathf.Clamp(Pay.Functions.Generic.GetMousePos(Camera.main).x, leftConfinerX, rightConfinerX), transform.position.y, transform.position.z);
    }
    public void OnLayerDragEnd(AbilityLayoutNodeLayer currentLayer)
    {
        currentState = LayoutState.Free;

        int currentLayerLayoutIndex = GetLayoutLayerIndex(currentLayer);
        int currentLayerStackIndex = currentLayer.RenderingLayer.LayerStackIndex;

        AbilityLayoutNodeLayer swapLayoutLayer = GetClosestLayoutLayer(currentLayer);
        int swapLayoutLayerIndex = GetLayoutLayerIndex(swapLayoutLayer);
        int swapLayerStackIndex = swapLayoutLayer.RenderingLayer.LayerStackIndex;

        if(currentLayerLayoutIndex != swapLayoutLayerIndex)
        {
            Ability.NodeLayer[] stackLayers = AttachedAbility.GetLayersInstance(); 
            SwapLayersInHierarchy(swapLayoutLayer, currentLayer);
            SwapLayersInStack(stackLayers[currentLayerStackIndex], stackLayers[swapLayerStackIndex]);
        }   UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    private void SwapLayersInHierarchy(AbilityLayoutNodeLayer first, AbilityLayoutNodeLayer second)
    {
        int firstSiblingIndex = first.transform.GetSiblingIndex();
        int secondSibIndex = second.transform.GetSiblingIndex();
        first.transform.SetSiblingIndex(secondSibIndex);
        second.transform.SetSiblingIndex(firstSiblingIndex);
    }
    private void SwapLayersInStack(Ability.NodeLayer first, Ability.NodeLayer second)
    {
        Ability.NodeLayer tempAbilityLayer = first;
        AttachedAbility.SetLayerAt(first.LayerStackIndex, second);
        AttachedAbility.SetLayerAt(second.LayerStackIndex, tempAbilityLayer);
    }
    private int GetLayoutLayerIndex(AbilityLayoutNodeLayer layer)
    {
        for(int i = 0; i < layoutLayers.Length; i++)
        {
            if(layer == layoutLayers[i]) return i;
        }
        return -1;
    }
    private AbilityLayoutNodeLayer GetClosestLayoutLayer(AbilityLayoutNodeLayer layer)
    {
        float closest = Mathf.Abs(layerPositionsBeforeDrag[0].x - layer.transform.position.x);
        int index = 0;
        for(int i = 0; i < layerPositionsBeforeDrag.Length; i++)
        {
            float curDist = Mathf.Abs(layerPositionsBeforeDrag[i].x - layer.transform.position.x);
            if(curDist < closest)
            {
                closest = curDist;
                index = i;
            }
        }
        return layoutLayers[index];
    }
    ///<summary>Returns the very left and right X positions.</summary>
    private Vector2 GetBounds()
    {
        float minX = layoutLayers[0].transform.position.x;
        int minIndex = 0;
        float maxx = layoutLayers[0].transform.position.x;
        int maxIndex = 0;
        for(int i = 0; i < layoutLayers.Length; i++)
        {
            float currentPosition = layoutLayers[i].transform.position.x;
            if(currentPosition < minX)
            {
                minIndex = i;
                minX = currentPosition;
            }
            else if(currentPosition > maxx)
            {
                maxIndex = i;
                maxx = currentPosition;
            }
        }
        return new Vector2(layoutLayers[minIndex].transform.position.x, layoutLayers[maxIndex].transform.position.x);
    }
}
