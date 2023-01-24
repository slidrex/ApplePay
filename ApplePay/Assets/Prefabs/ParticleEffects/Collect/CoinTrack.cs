using UnityEngine;

public class CoinTrack : TrackAnim
{
    [HideInInspector] public Animator CurrencyRendererAnim;
    public CurrencyHolder holder;
    public CollectableCoin coin;
    protected override void OnAimDestinate()
    {
        CurrencyRendererAnim.SetTrigger("Collected");
        holder.AddAmount(CurrencyHolder.Currency.Money, coin.Amount);
        base.OnAimDestinate();
    }
}
