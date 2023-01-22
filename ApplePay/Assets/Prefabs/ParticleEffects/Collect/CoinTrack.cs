using UnityEngine;

public class CoinTrack : TrackAnim
{
    [HideInInspector] public Animator CurrencyRendererAnim;
    protected override void AimDestinate()
    {
        CurrencyRendererAnim.SetTrigger("Collected");
        base.AimDestinate();
    }
}
