[UnityEngine.CreateAssetMenu(menuName = "Item/Charm/Mixed Charm", fileName = "new charm")]

public class MixedCharm : CharmObject
{
    public Charm[] Charms;
    private void Awake()
    {
        for(int i = 0; i < Charms.Length; i++)
        {
            Charms[i] = Instantiate(Charms[i]);
        }
    }
    [UnityEngine.HideInInspector] public byte ActiveIndex;
}
