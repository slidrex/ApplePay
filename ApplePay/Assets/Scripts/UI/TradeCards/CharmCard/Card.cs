using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text description;
    [SerializeField] private Text additionalField;
    [SerializeField] private Image icon;
    [SerializeField] private int cardNumber;
    private CardSpawner cardSpawner;
    private Animator anim;
    //private int Price;
    private void Start()
    {
        anim = GetComponent<Animator>();
        cardSpawner = GetComponentInParent<CardSpawner>();
    }
    public void SetCardInfo(string itemName, string description, Sprite icon)
    {
        this.itemName.text = itemName;
        this.description.text = description;
        //this.additionalField.text = additionalField;
        this.icon.sprite = icon;
    }
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetTrigger("Exit");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Instantiate(cardSpawner.charmDatabase.GetItem(cardSpawner.uniqueIndexes[cardNumber]),
         FindObjectOfType<PlayerEntity>().transform.position, Quaternion.identity);
    }
}