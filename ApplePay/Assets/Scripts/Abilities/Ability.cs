using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Ability/Node-based Ability")]
public class Ability : ScriptableObject
{
    public delegate void AbilityEndCallback(Ability ability);
    private AbilityEndCallback abilityEndCallback;
    public float timeSinceAbilityActivated { get; set; }
    [SerializeField] private NodeLayer[] NodeLayers;
    private int activatingNodeLayerIndex;
    private int currentLayerNodeCount;
    private int executedCurrentLayerNodeCount;
    private Creature executer;
    public bool IsActivate { get; set; }
    public NodeLayer[] GetLayersInstance() 
    {
        NodeLayer[] layers = new NodeLayer[NodeLayers.Length];
        layers = NodeLayers;
        return layers;
    }
    public void OnInstantiated()
    {
        for(int i = 0; i < NodeLayers.Length; i++)
        {
            NodeLayer curLayer = NodeLayers[i];
            curLayer.AttachedAbility = this;
            curLayer.LayerStackIndex = i;
            for(int j = 0; j < curLayer.Nodes.Length; j++)
            {
                AbilityNode currentNode = NodeLayers[i].Nodes[j];
                if(currentNode != null)
                {
                    NodeLayers[i].Nodes[j] = Instantiate(curLayer.Nodes[j]);
                    currentNode.AttachedLayer = curLayer;
                    currentNode.NodeStackIndex = j;
                }
            }
        }
    }
    public void SetLayerAt(int index, NodeLayer layer)
    {
        NodeLayers[index] = layer;
        layer.LayerStackIndex = index;
    }
    public void SetNodeAt(int layerIndex, int nodeIndex, AbilityNode node)
    {
        NodeLayers[layerIndex].Nodes[nodeIndex] = node;
        if(node != null)
        {
            node.NodeStackIndex = nodeIndex;
            node.AttachedLayer = NodeLayers[layerIndex];
        }
    }
    public AbilityNode GetNodeAt(int layerIndex, int nodeIndex) => NodeLayers[layerIndex].Nodes[nodeIndex];
    public virtual void OnRepositoryAdded()
    {
        timeSinceAbilityActivated = 0.0f;
    }
    public void BeginAbility(Creature executer, AbilityEndCallback callback)
    {
        this.executer = executer;
        timeSinceAbilityActivated = 0.0f;
        abilityEndCallback += callback;
        IsActivate = true;
    }
    private void AbilityEnd()
    {
        executer = null;
        abilityEndCallback?.Invoke(this);
        abilityEndCallback = null;
        IsActivate = false;
    }
    public void OnUpdate()
    {
        if(IsActivate) ProcessNodes();
        else CalculateCooldown();
    }
    private void CalculateCooldown() 
    {
        if(timeSinceAbilityActivated < GetCooldown()) timeSinceAbilityActivated += Time.deltaTime;
    }
    private void ProcessNodes()
    {
        bool nodeUpdate = false;
        foreach(AbilityNode layerNode in NodeLayers[activatingNodeLayerIndex].Nodes)
        {
            if(layerNode != null)
            {
                if(layerNode.isInitialized == false) 
                {
                    layerNode.BeginNode(executer, this, OnNodeExecuted);
                    currentLayerNodeCount++;
                }
                nodeUpdate = layerNode.UpdateNode();
            }
        }
        if(currentLayerNodeCount == 0 && !nodeUpdate) 
        {
            MoveNextLayer();
        }
    }
    private void OnNodeExecuted()
    {
        executedCurrentLayerNodeCount++;
        
        if(executedCurrentLayerNodeCount >= currentLayerNodeCount)
        {
            MoveNextLayer();
        }
    }
    private void MoveNextLayer()
    {
        executedCurrentLayerNodeCount = 0;
        currentLayerNodeCount = 0;
        
        if(activatingNodeLayerIndex < NodeLayers.Length - 1)
        {
            activatingNodeLayerIndex++;
            Debug.Log("new layer " + activatingNodeLayerIndex);
        }
        else
        {
            activatingNodeLayerIndex = 0;
            AbilityEnd();
        }
    }
    public bool IsActivatable() => timeSinceAbilityActivated >= GetCooldown();
    public int GetEnergyCost()
    {
        int energyCost = 0;
        foreach(NodeLayer layer in NodeLayers)
        {
            foreach(AbilityNode node in layer.Nodes)
            {
                if(node != null)
                    energyCost += node.GetNodeEnergyCost();
            }
        }
        return energyCost;
    }
    public float GetCooldown()
    {
        float cooldown = 0.0f;
        foreach(NodeLayer layer in NodeLayers)
        {
            foreach(AbilityNode node in layer.Nodes)
            {
                if(node != null)
                    cooldown += node.GetNodeCooldown();
            }
        }
        return cooldown;
    }
    [System.Serializable]
    public struct NodeLayer
    {
        public Ability AttachedAbility { get; internal set; }
        public int LayerStackIndex { get; internal set; }
        public AbilityNode[] Nodes;
    }
}
