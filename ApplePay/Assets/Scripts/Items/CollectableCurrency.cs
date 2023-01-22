public class CollectableCurrency : CollectableMillestone
{
    public CurrencyHolder.Currency CurrencyType;
    public int Amount;
    protected override bool CollectRequest(HitInfo hitInfo)
    {
        CurrencyHolder currentHolder = hitInfo.entity?.GetComponent<CurrencyHolder>();
        bool requestStatus = currentHolder != null && currentHolder.AddAmount(CurrencyType, Amount);
        
        return requestStatus; 
    }
    protected override void OnCollect(HitInfo hitInfo)
    {
        if(hitInfo.entity.GetComponent<PlayerEntity>() != null && TrackAnim != null)
        {
            CurrencyHolder holder = hitInfo.entity.GetComponent<CurrencyHolder>();
            CoinTrack obj = Instantiate((CoinTrack)TrackAnim, transform.position, UnityEngine.Quaternion.identity);
            obj.DestinationPoint = holder.openAccounts[0].MagnitizedObject;
            obj.CurrencyRendererAnim = holder.openAccounts[0].Animator;
            base.OnCollect(hitInfo);
        }
        else base.OnCollect(hitInfo);
    }
}
