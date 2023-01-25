public class CollectableCharm : CollectableItem<CollectableCharm>
{
    protected override bool DestroyOnCollect => false;
    public override CollectableCharm CollectableObject { get => this; }
    protected override string hoverableObjectHeader => Charm.GetActiveCharm().Display.Description.Name;
    protected override string hoverableObjectDescription => Charm.GetActiveCharm().Display.Description.Description;
    [UnityEngine.SerializeField] private CharmObject charm;
    public CharmObject Charm { get; set; }
    protected override string TargetRepository => "charm";
    protected override void Awake()
    {
        Charm = Instantiate(charm);
        Charm.OnInstantiate();
        base.Awake();
    }
}