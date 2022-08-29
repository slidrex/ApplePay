using UnityEngine;

public class HearthBonus : CollectableBonus
{
    [Header("Hearth Bonus")]
    public GameObject BonusAnimObj;
    public int HealAmount;
    public override void Collect(Collider2D collision, ref bool picked)
    {
        picked = true;
        Transform magnitizeObj = null;
        Animator hbAnim = null;
        if(collision.GetComponent<Creature>().HealthBar != null)
        {
            magnitizeObj = collision.GetComponent<Creature>().HealthBar.MagnitizedObj;
            hbAnim = collision.GetComponent<Creature>().HealthBar.Animator;
        }
        if(magnitizeObj != null) 
        {
            var obj = Instantiate(BonusAnimObj, transform.position, Quaternion.identity);
            HeartAnim ha = obj.GetComponent<HeartAnim>();
            ha.DestinationPoint = magnitizeObj;
            ha.HealthBarAnim = hbAnim;
            ha.EntityApply = collision.GetComponent<Creature>();
            ha.HealAmount = HealAmount;
        }
        else
        {
            collision.GetComponent<Creature>().ChangeHealth(HealAmount);
        }
        base.Collect(collision, ref picked);
    }
}
