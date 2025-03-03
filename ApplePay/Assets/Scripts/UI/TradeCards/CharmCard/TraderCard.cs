using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraderCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject applause;
    [SerializeField] private Text itemName;
    [SerializeField] private Text description;
    [SerializeField] private Text textField;
    [SerializeField] private Transform additionalFields;
    [SerializeField] private Image icon;
    [SerializeField] private Image qualityFrame;
    private CardSpawner cardSpawner;
    private byte itemIndex;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        cardSpawner = GetComponentInParent<CardSpawner>();
    }
    public void LoadItem(byte index) => itemIndex = index;
    public void SetHeader(string header, Color color, Sprite icon)
    {
        this.itemName.text = header;
        this.itemName.color = color;
        this.icon.sprite = icon;
    }
    public void SetQuality(ItemRarity rarity) 
    {
        qualityFrame.color = ItemRarityExtension.GetRarityInfo(rarity).color;
    }
    public void SetDescription(string text)
    {
        description.text = text;
    }
    public void AddField(string text, Color color)
    {
        Text _text = Instantiate(textField);
        _text.transform.SetParent(additionalFields);
        _text.transform.localPosition = Vector3.zero;
        _text.transform.localScale = Vector3.one;
        _text.text = text;
        _text.color = color;
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
        anim.SetTrigger("Click");
    }
    private void SetActive(bool isActive) => gameObject.SetActive(isActive);
    private void Disable()
    {
        int hardcode;
        Instantiate(cardSpawner.charmDatabase.GetItem(itemIndex),
            FindObjectOfType<PlayerEntity>().transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
        PayWorld.Particles.InstantiateParticles(applause, transform.position, Quaternion.identity, 2, cardSpawner.transform);
        
        gameObject.SetActive(false);
    }
}