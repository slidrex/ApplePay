using UnityEngine;

public class CurrencyHolder : MonoBehaviour
{
    public delegate void AccountValueChangeCallback(Currency changedAccount, int value);
    public AccountValueChangeCallback AccountChangeCallback;
    [SerializeField] private CurrencyAccount[] openAccounts;
    public bool ContainsAccount(Currency currecncyType)
    {
        foreach(CurrencyAccount acc in openAccounts)
        {
            if(acc.currencyType == currecncyType) return true;
        }
        return false;
    }
    public bool AddAmount(Currency account, int amount)
    {
        for(int i = 0; i < openAccounts.Length; i++)
        {
            if(openAccounts[i].currencyType == account)
            {
                openAccounts[i].Amount += amount;
                AccountChangeCallback.Invoke(account, amount);
                return true;
            }
        }
        return false;
    }
    public int GetAccountValue(Currency account)
    {
        foreach(CurrencyAccount acc in openAccounts)
        {
            if(acc.currencyType == account) return acc.Amount;
        }
        throw new System.NullReferenceException("Account doesn't exist.");
    }
    [System.Serializable]
    public struct CurrencyAccount
    {
        public Currency currencyType;
        public int Amount;
    }
    public enum Currency
    {
        Money,
        RainbowShards
    }
}
