using UnityEngine;

public class AbilityNodeLayout : MonoBehaviour
{
    [SerializeField] private AbilityLayoutNodeLayer layer;
    private AbilityLayoutNodeLayer[] layers;
    private float leftConfinerX;
    private float rightConfinerX;
    private RectTransform rectTransform;
    private Vector2[] initialPoses;
    private Ability attachedAbility;
    public AbilityCanvasRenderer canvasRenderer { get; private set; }
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
        attachedAbility = ability;
        if(layers != null)
            foreach(AbilityLayoutNodeLayer currentLayer in layers)
                Destroy(currentLayer);
        
        layers = new AbilityLayoutNodeLayer[ability.NodeLayers.Length];
        for(int i = 0; i < ability.NodeLayers.Length; i++)
        {
            AbilityLayoutNodeLayer curLayer = Instantiate(layer, transform);
            curLayer.LinkLayout(this);
            curLayer.BuildLayer(ability.NodeLayers[i]);
            layers[i] = curLayer;
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
    public void OnNodePointerEnter(AbilityLayoutNode node)
    {
        node.SetActiveMask(true);
    }
    public void OnNodePointerExit(AbilityLayoutNode node)
    {
        node.SetActiveMask(false);
    }
    public void OnLayerDragBegin(AbilityLayoutNodeLayer layer)
    {
        initialPoses = new Vector2[layers.Length];
        for(int i = 0; i < layers.Length; i++) initialPoses[i] = layers[i].transform.position;
        rightConfinerX = layers[layers.Length - 1].transform.position.x;
        leftConfinerX = layers[0].transform.position.x;
    }
    public void OnLayerDrag(AbilityLayoutNodeLayer layer)
    {
        layer.transform.position = new Vector3(Mathf.Clamp(Pay.Functions.Generic.GetMousePos(Camera.main).x, leftConfinerX, rightConfinerX), transform.position.y, transform.position.z);
    }
    public void OnLayerDragEnd(AbilityLayoutNodeLayer layer)
    {
        int layerIndex = GetLayerIndex(layer);
        float closest = Mathf.Abs(initialPoses[0].x - layer.transform.position.x);
        int swapIndex = 0;
        for(int i = 0; i < initialPoses.Length; i++)
        {
            float curDist = Mathf.Abs(initialPoses[i].x - layer.transform.position.x);
            if(curDist < closest)
            {
                closest = curDist;
                swapIndex = i;
            }
        }
        if(layerIndex != swapIndex)
        {
            int curSiblingIndex = layer.transform.GetSiblingIndex();
            int swapSibIndex = layers[swapIndex].transform.GetSiblingIndex();
            layer.transform.SetSiblingIndex(swapSibIndex);
            layers[swapIndex].transform.SetSiblingIndex(curSiblingIndex);
            AbilityLayoutNodeLayer tempLayer = layers[layerIndex];
            layers[layerIndex] = layers[swapIndex];
            layers[swapIndex] = tempLayer;
            Ability.NodeLayer tempAbilityLayer = attachedAbility.NodeLayers[layerIndex];
            attachedAbility.NodeLayers[layerIndex] = attachedAbility.NodeLayers[swapIndex];
            attachedAbility.NodeLayers[swapIndex] = tempAbilityLayer;
        }
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    private int GetLayerIndex(AbilityLayoutNodeLayer layer)
    {
        for(int i = 0; i < layers.Length; i++)
        {
            if(layer == layers[i]) return i;
        }
        return -1;
    }
}
