using UnityEngine;

public abstract class AbilityNode : ScriptableObject
{
    public NodeDisplay Display;
    public delegate void OnNodeProcessed();
    private OnNodeProcessed onNodeProcessedCallback;
    [SerializeField] protected float NodeTime;
    private float timeSinceNodeActivated;
    [SerializeField] protected NodeField[] Fields;
    private Ability processedAbility;
    private Creature executer;
    public bool isInitialized { get; private set; }
    public float GetNodeCooldown() 
    {
        float cooldown = 0.0f;
        foreach(NodeField field in Fields)
        {
            cooldown += field.FieldScore * field.CooldownPerScore;
        }
        if(cooldown < 0) throw new System.Exception("Cooldown cannot be less than zero!");
        return cooldown;
    }
    public int GetNodeEnergyCost()
    {
        int cost = 0;
        foreach(NodeField field in Fields)
        {
            cost += field.FieldScore * field.EnergyCostPerScore;
        }
        return cost;
    }
    public void BeginNode(Creature executer, Ability ability, OnNodeProcessed endCallback)
    {
        isInitialized = true;
        timeSinceNodeActivated = 0.0f;
        processedAbility = ability;
        this.executer = executer;
        onNodeProcessedCallback += endCallback;
        OnNodeBegin(executer);
    }
    public void UpdateNode()
    {
        if(timeSinceNodeActivated < NodeTime)
        {
            timeSinceNodeActivated += Time.deltaTime;
            OnNodeUpdate(executer);
        } else if(isInitialized)
        {
            OnNodeEnd(executer);
            onNodeProcessedCallback.Invoke();
            onNodeProcessedCallback = null;
            isInitialized = false;
        }
    }
    protected virtual void OnNodeBegin(Creature entity) {}
    protected virtual void OnNodeUpdate(Creature entity) {}
    protected virtual void OnNodeEnd(Creature entity) {}
    protected float GetNodeValue(string name)
    {
        NodeField field = GetNodeField(name);
        return field.FieldScore * field.ScoreMultiplier;
    }
    protected NodeField GetNodeField(string name)
    {
        foreach(NodeField field in Fields)
        {
            if(field.FieldName == name)
            {
                return field;
            }
        }
        throw new System.NullReferenceException("Field \"" + name + "\" doesn't exist but you are trying to access it.");
    }
    [System.Serializable]
    public struct NodeField
    {
        public string FieldName;
        public int FieldScore;
        public float ScoreMultiplier;
        public int EnergyCostPerScore;
        public float CooldownPerScore;
    }
}
