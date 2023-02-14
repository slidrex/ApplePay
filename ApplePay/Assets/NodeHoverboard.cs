using UnityEngine.UI;
using UnityEngine;

public class NodeHoverboard : MonoBehaviour
{
    [SerializeField] private Text additionalFieldText;
    [SerializeField] private Transform additionalFieldsContainer;
    [Header("Constant Fields")]
    [SerializeField] private Text nameTag;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text cooldownText;
    [SerializeField] private Text energyCostText;
    public void SetName(string text)
    {
        nameTag.text = text;
    }
    public void SetCooldown(int amount)
    {
        cooldownText.text = amount.ToString();
    }
    public void SetEnergyCost(int amount)
    {
        energyCostText.text = amount.ToString();
    }
    public void SetDescription(string text)
    {
        descriptionText.text = text;
    }
    public void AddStatField(string statName, float amount, bool showSign, bool percent = false)
    {
        var field = Instantiate(additionalFieldText, additionalFieldsContainer.transform.position, Quaternion.identity, additionalFieldsContainer);
        string prefix = showSign ? amount > 0 ? "+" : "i" : "";
        string postfix = "";
        if(percent)
        {
            postfix = "%";
            amount *= 100;
        }
        field.text = statName + ":" + " " + prefix + amount + postfix;
    }

}
