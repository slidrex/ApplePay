using Pay.UI;
using UnityEngine;
public class BossEntity : AttackingMob, IUIHolder
{
    [SerializeField] private BossBar bossBar;
    public static ListOfBossBars listOfBossBarsInstance;
    public BossBar bossBarInstance {get; set;}
    private UIHolder holder;

    protected override void Start()
    {
        base.Start();
        ListOfBossBars listOfBossBars = Resources.Load<ListOfBossBars>("ListOfBossBars");
        if(listOfBossBars == null) throw new System.Exception("LIST OF BOSS BARS NOT FOUND!"); 
        if(bossBar != null && listOfBossBars != null)
        {
            holder = FindObjectOfType<UIHolder>();
            if(ListOfBossBars.IsHave == false)
            {
                ListOfBossBars.IsHave = true;
                listOfBossBarsInstance = Instantiate(listOfBossBars, holder.HUDCanvas.transform.position, Quaternion.identity, holder.HUDCanvas.transform);
            }
        }
        ListOfBossBars.AddBossBar(this);
    }
    public override void Damage(Creature handler, params Damage[] damage)
    {
        base.Damage(handler, damage);
        if(bossBarInstance != null) 
        {
            bossBarInstance.GetValue().fillAmount = (float)CurrentHealth / (float)MaxHealth;
        }
    }
    protected override void ApplyDamage(Creature handler)
    {
        base.ApplyDamage(handler);
        bossBarInstance?.animator?.SetTrigger("TakeDamage");
    }
    protected override void Die(Creature killer)
    {
        ListOfBossBars.DestroyBossBar(this);
        base.Die(killer);
    }
    public BossBar GetBossBar() => bossBar;
    public UIHolder GetHolder() => holder;
}
