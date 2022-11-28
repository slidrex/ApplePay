using UnityEngine;

public class HearthBonus : CollectableBonus
{
    [Header("Hearth Bonus")]
    public GameObject BonusAnimObj;
    public int HealAmount;
    public override void CollisionRequest(HitInfo collision, ref bool collectStatus)
    {
        Creature picker = collision.entity.GetComponent<Creature>();
        if(picker == null) return;
        collectStatus = true;
        Transform magnitizeObj = null;
        Animator hbAnim = null;
        if(picker.HealthBar != null)
        {
            magnitizeObj = picker.HealthBar.MagnitizedObj;
            hbAnim = picker.HealthBar.Animator;
        }
        if(magnitizeObj != null) 
        {
            var obj = Instantiate(BonusAnimObj, transform.position, Quaternion.identity);
            HeartAnim ha = obj.GetComponent<HeartAnim>();
            ha.DestinationPoint = magnitizeObj;
            ha.HealthBarAnim = hbAnim;
            ha.EntityApply = picker;
            ha.HealAmount = HealAmount;
        }
        else
        {
            collision.entity.GetComponent<Creature>().ChangeHealth(HealAmount);
        }
        SendCollectRequest(collision, collectStatus);
    }
}
