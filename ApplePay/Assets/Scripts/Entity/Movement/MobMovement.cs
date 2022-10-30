using UnityEngine;

public class MobMovement : EntityMovement
{
    [SerializeField] private Transform target;
    [Header("Movement Patterns")]
    public System.Collections.Generic.List<MovePatterns> Patterns = new System.Collections.Generic.List<MovePatterns>();
    [SerializeField] private byte[] activePatterns = new byte[1];
    private MovePatterns[] GetActivePatterns()
    {
        MovePatterns[] patterns = new MovePatterns[activePatterns.Length];
        for(int i = 0; i < activePatterns.Length; i++)
        {
            byte index = activePatterns[i];
            if(index > Patterns.Count) throw new System.Exception("Specified index is out of array bonds.");
            if(Patterns[index] == null) throw new System.NullReferenceException("Movement pattern with index " + index + " doesn't exist.");
            patterns[i] = Patterns[index];
        }
        return patterns;
    }
    protected void SetTarget(Transform target) => this.target = target;
    protected override void Start()
    {
        base.Start();
        InitPatterns();
    }
    protected void InitPatterns()
    {
        foreach(MovePatterns movePattern in Patterns) movePattern.Init(this, transform, target);
    }
    protected override void Update()
    {
        base.Update();
        if(!isDisabled) foreach(MovePatterns movePattern in GetActivePatterns()) movePattern.OnUpdate();
    }
    protected override void OnSpeedUpdate()
    {
        if(!isDisabled) foreach(MovePatterns movePattern in GetActivePatterns()) movePattern.OnSpeedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(!isDisabled) foreach(MovePatterns movePattern in GetActivePatterns()) movePattern.OnCollision(collision);
    }
}