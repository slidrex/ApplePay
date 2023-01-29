[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Mixed Charm", fileName = "new charm")]

public class MixedCharm : CharmObject
{
    [UnityEngine.SerializeField] private Charm[] charms;
    public Charm[] Charms {get; set;}
    public override void OnInstantiate()
    {
        base.OnInstantiate();
        int charmLength = charms.Length;
        Charms = new Charm[charmLength];
        for(int i = 0; i < charmLength; i++)
        {
            Charms[i] = Instantiate(charms[i]);
            Charms[i].OnInstantiate();
        }
    }
    [UnityEngine.HideInInspector] public byte ActiveIndex;
}