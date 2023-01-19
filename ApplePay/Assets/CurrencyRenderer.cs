using UnityEngine;

public class CurrencyRenderer : MonoBehaviour, ICurrencyAccountUpdateCallback
{
    [SerializeField] private CurrencyHolder holder;
    [SerializeField] private CurrencyHolder.Currency currencyType;
    [SerializeField] private UnityEngine.UI.Text currencyAmount;
    private void Awake()
    {
        UpdateCurrencyCounter();
        holder.AccountChangeCallback += AccountUpdateCallback;}
    public void AccountUpdateCallback(CurrencyHolder.Currency accountType, int amount)
    {
        if(accountType == currencyType) UpdateCurrencyCounter();
    }
    private void UpdateCurrencyCounter()
    {
        currencyAmount.text = holder.GetAccountValue(currencyType).ToString();
    }
}
