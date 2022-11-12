using UnityEngine;

public class MobMovement : EntityMovement
{
    public Transform Target { get; private set; }
    public new MobEntity Entity {get => (MobEntity)base.Entity; }
    [Header("Movement Patterns")]
    public System.Collections.Generic.List<MovementPattern> Patterns = new System.Collections.Generic.List<MovementPattern>();
    [SerializeField] private byte[] activePatterns = new byte[1];
    private MovementPattern[] GetActivePatterns()
    {
        MovementPattern[] patterns = new MovementPattern[activePatterns.Length];
        for(int i = 0; i < activePatterns.Length; i++)
        {
            byte index = activePatterns[i];
            if(index > Patterns.Count) throw new System.Exception("Specified index is out of array bonds.");
            if(Patterns[index] == null) throw new System.NullReferenceException("Movement pattern with index " + index + " doesn't exist.");
            patterns[i] = Patterns[index];
        }
        return patterns;
    }
    public void SetMovementTarget(Transform target) => Target = target;
    public void SetActivePatterns(params byte[] patterns) => activePatterns = patterns;
    protected override void Start()
    {
        base.Start();
        InitPatterns();
    }
    protected void InitPatterns()
    {
        foreach(MovementPattern movePattern in Patterns) movePattern.Init(this, transform);
    }
    protected override void Update()
    {
        if(Target == null) 
        {
            Target = Entity.Target != null ? Entity.Target.transform : LevelController.EntityTagHandler.GetNearestHostile(Entity.CurrentRoom, Entity).transform;
        }
        base.Update();
        if(!isDisabled) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnUpdate();
    }
    protected override void OnSpeedUpdate()
    {
        if(!isDisabled) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnSpeedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(!isDisabled) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnCollision(collision);
    }
}