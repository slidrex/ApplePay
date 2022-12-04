using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject applause;
    [SerializeField] private Text itemName;
    [SerializeField] private Text description;
    [SerializeField] private Text additionalField;
    [SerializeField] private Image icon;
    [SerializeField] private byte cardNumber;
    [SerializeField] private Text sold;
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
        /*Instantiate(cardSpawner.charmDatabase.GetItem(cardSpawner.uniqueIndexes[cardNumber]),
         FindObjectOfType<PlayerEntity>().transform.position, Quaternion.identity);*/
        anim.SetTrigger("Click");
    }
    private void SetActive(bool isActive) => gameObject.SetActive(isActive);
    private void Disable()
    {
        PayWorld.Particles.InstantiateParticles(applause, transform.position, Quaternion.identity, 2, cardSpawner.transform);
        sold.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}