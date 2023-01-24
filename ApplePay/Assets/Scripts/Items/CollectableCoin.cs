public class CollectableCoin : CollectableCurrency
{
    protected override bool CollectRequest(HitInfo hitInfo)
    {
        if(hitInfo.entity as PlayerEntity != null && TrackAnim != null)
        {
            CurrencyHolder holder = hitInfo.entity.GetComponent<CurrencyHolder>();
            CoinTrack obj = Instantiate((CoinTrack)TrackAnim, transform.position, UnityEngine.Quaternion.identity);
            CurrencyRenderer renderer = holder.GetAccount(CurrencyHolder.Currency.Money).OnCurrencyCollect;
            obj.DestinationPoint = renderer.transform;
            obj.CurrencyRendererAnim = renderer.GetComponent<UnityEngine.Animator>();
            obj.coin = this;
            obj.holder = holder;
        }
        base.OnCollect(hitInfo);
        return true;
    }
    protected override void OnCollect(HitInfo hitInfo)
    {
        
    }
}
