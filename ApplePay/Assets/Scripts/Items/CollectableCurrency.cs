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
}
