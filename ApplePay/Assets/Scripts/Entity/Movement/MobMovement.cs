using UnityEngine;

public class MobMovement : EntityMovement
{
    public Transform Target;
    [Header("Movement Patterns")]
    public System.Collections.Generic.List<MovementPattern> Patterns = new System.Collections.Generic.List<MovementPattern>();
    private bool disablePatterns;
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
    public void SetActivePatterns(params byte[] patterns) => activePatterns = patterns;
    public void DisablePatterns(bool disable)
    {
        disablePatterns = disable;
        if(disable) MoveVector = Vector2.zero;
    }
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
        base.Update();
        if(!isDisabled && !disablePatterns) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnUpdate();
    }
    protected override void OnSpeedUpdate()
    {
        if(!isDisabled && !disablePatterns) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnSpeedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(!isDisabled && !disablePatterns) foreach(MovementPattern movePattern in GetActivePatterns()) movePattern.OnCollision(collision);
    }
}