using UnityEngine;

public class ReaperBehaviour : AttackingMob
{
    [Header("Death Behaviour")]
    [SerializeField] private GameObject SummonObject;
    [SerializeField] private float StartSummonDelay;
    [SerializeField] private float SummonDelay;
    private GameObject ObjectContainer;
    private float curSummonDelay;
    protected override void Start()
    {
        base.Start();
        SetTarget(GameObject.FindGameObjectWithTag("Player").GetComponent<Creature>());
        curSummonDelay = StartSummonDelay;
    }
    protected override void Update()
    {
        if(curSummonDelay > 0 && ObjectContainer == null)
        {
            curSummonDelay -= Time.deltaTime;
        } else if(ObjectContainer == null)
        {
            ObjectContainer = Instantiate(SummonObject, transform.position, Quaternion.identity);
            ObjectContainer.GetComponent<ScytheBehaviour>().SetTarget(Target);
            curSummonDelay = SummonDelay;
        }
    }
}
