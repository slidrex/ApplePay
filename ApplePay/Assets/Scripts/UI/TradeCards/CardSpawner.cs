using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    private TraderCard[] cards;
    [SerializeField] private TraderCard card;
    public CharmDatabase charmDatabase;
    [SerializeField] private Transform cardHolder;
    private System.Collections.Generic.List<byte> usedCardIndeces = new System.Collections.Generic.List<byte>();
    [SerializeField] private byte minCardCount;
    [SerializeField] private byte maxCardCount;
    [HideInInspector] public byte CardCount;
    private int selectedCardIndex;
    private bool inAnimation;
    private Vector2 TargetDistance;
    private void Start()
    {
        CardCount = (byte)Random.Range(minCardCount, maxCardCount + 1);
        cards = new TraderCard[CardCount];
        SpawnCards();
        selectedCardIndex = CardCount / 2;
        cardHolder.transform.position  += (cards[selectedCardIndex].transform.position - transform.position);
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow) && selectedCardIndex < CardCount - 1 && inAnimation == false) 
        {
            selectedCardIndex++;
            BeginAnimation(cardHolder.position - (cards[selectedCardIndex].transform.position - transform.position));
        }
        if(Input.GetKey(KeyCode.LeftArrow) && selectedCardIndex > 0 && inAnimation == false)
        {
            selectedCardIndex--;
            BeginAnimation(cardHolder.position - (cards[selectedCardIndex].transform.position - transform.position));
        }
        if(inAnimation) OnAnimation();
    }
    private void BeginAnimation(Vector2 targetDistance)
    {
        inAnimation = true;
        TargetDistance = targetDistance;
    }
    private void OnAnimation()
    {
        cardHolder.transform.position = Vector3.MoveTowards(cardHolder.transform.position, TargetDistance, 10f * Time.deltaTime);
        if((Vector2)cardHolder.transform.position == TargetDistance) inAnimation = false;
    }
    private void SpawnCards()
    {
        for (int i = 0; i < CardCount; i++)
        {
            byte index = GetUniqueIndex();
            cards[i] = Instantiate(card, Vector3.zero, Quaternion.identity, cardHolder);
            cards[i].transform.localPosition = Vector3.zero;
            cards[i].LoadItem(index);
            CollectableCharm collectableItem = charmDatabase.GetItem(index);
            usedCardIndeces.Add(index);
            ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(collectableItem.charm.Display.Rarity);
            cards[i].SetHeader(collectableItem.charm.Display.Description.Name, rarityInfo.color, collectableItem.charm.Display.Icon);
            cards[i].SetDescription(collectableItem.charm.Display.Description.Description);
            cards[i].SetQuality(collectableItem.charm.Display.Rarity);
            for(int j = 0; j < collectableItem.charm.Display.AdditionalFields.Length; j++)
            {
                
                cards[i].AddField(collectableItem.charm.GetActiveCharm().Display.AdditionalFields[j].Text, collectableItem.charm.Display.AdditionalFields[j].GetColor());
            }
        }
    }
    private byte GetUniqueIndex()
    {
        bool generated = false;
        
        while(generated == false)
        {
            byte index = (byte)Random.Range(0, charmDatabase.GetLength());
            if(usedCardIndeces.Contains(index) == false) return index;
        }
        return 0;
    }
}
