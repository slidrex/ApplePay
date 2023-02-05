using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Ability/Node-based Ability")]
public class Ability : ScriptableObject
{
    public delegate void AbilityEndCallback(Ability ability);
    private AbilityEndCallback abilityEndCallback;
    private float timeSinceAbilityActivated;
    [SerializeField] protected AbilityNode[] Nodes;
    private int activatingNodeIndex;
    private Creature executer;
    public bool IsActivate { get; set; }
    public void OnInstantiated()
    {
        for(int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i] = Instantiate(Nodes[i]);
        }
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
        AbilityNode currentNode = Nodes[activatingNodeIndex];
        if(currentNode.isInitialized == false) currentNode.BeginNode(executer, this, OnNodeExecuted);
        currentNode.UpdateNode();
    }
    private void OnNodeExecuted()
    {
        if(activatingNodeIndex + 1 < Nodes.Length)
        {
            activatingNodeIndex++;
        }
        else 
        {
            IsActivate = false;
            AbilityEnd();
            activatingNodeIndex = 0;
        }

    }
    public bool IsActivatable() => timeSinceAbilityActivated >= GetCooldown();
    public int GetEnergyCost()
    {
        int energyCost = 0;
        foreach(AbilityNode node in Nodes)
        {
            energyCost += node.GetNodeEnergyCost();
        }
        return energyCost;
    }
    public float GetCooldown()
    {
        float cooldown = 0.0f;
        foreach(AbilityNode node in Nodes)
        {
            cooldown += node.GetNodeCooldown();
        }
        return cooldown;
    }
}
