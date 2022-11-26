public class CollectableCharm : CollectableItem<Charm>
{
    [field: UnityEngine.SerializeField] protected override Charm CollectableObject {get;set;}
    protected override string TargetRepository => "charm";
}
