public class CollectableCharm : CollectableItem<Charm>
{
    protected override Charm CollectableObject {get => charm; set => charm = value;}
    public Charm charm;
    protected override string TargetRepository => "charm";
}
