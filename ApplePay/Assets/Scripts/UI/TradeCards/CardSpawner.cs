using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    private TraderCard[] cards;
    [SerializeField] private TraderCard card;
    public CharmDatabase charmDatabase;
    [SerializeField] private Transform cardHolder;
    public System.Collections.Generic.List<byte> usedCardIndeces = new System.Collections.Generic.List<byte>();
    [SerializeField] private byte minCardCount;
    [SerializeField] private byte maxCardCount;
    [HideInInspector] public byte CardCount;
    private int selectedCardIndex;
    private CardAnimation cardAnimation;
    private const string animationSelectTrigger = "Select";
    private const string animationDeselectTrigger = "Deselect";
    private bool initAmbigous;
    private void Start()
    {
        CardCount = (byte)Random.Range(minCardCount, maxCardCount + 1);
        cards = new TraderCard[CardCount];
        cardAnimation = new CardAnimation();
        SpawnCards();
    }
    private void OnEnable() => InitPosition();
    private void InitPosition()
    {
        cardHolder.position = transform.position;
        selectedCardIndex = selectedCardIndex = CardCount / 2;
        if(CardCount % 2 == 0) initAmbigous = true;
        else cards[selectedCardIndex].anim.SetTrigger(animationSelectTrigger);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            int previousIndex = selectedCardIndex;
            SwitchActiveCard(true);
            
            if(selectedCardIndex != previousIndex || initAmbigous)
            {
                StartAnimation(previousIndex);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int previousIndex = selectedCardIndex;
            SwitchActiveCard(false);
            if(selectedCardIndex != previousIndex || initAmbigous) 
            {
                StartAnimation(previousIndex);
            }
        }
        if(cardAnimation.InAnimation()) OnAnimation();
    }
    private void SwitchActiveCard(bool right)
    {
        if(initAmbigous == false)
        {
            if(right && selectedCardIndex < CardCount - 1) selectedCardIndex++;
            else if(right == false && selectedCardIndex > 0) selectedCardIndex--;
        }
        else
        {
            if(right == false) selectedCardIndex --; 
        }

    }
    private Vector2 GetTargetDistance() 
    {
        return cardHolder.transform.position - (cards[selectedCardIndex].transform.position - transform.position);
    }
    private void StartAnimation(int previousIndex)
    {
        if(cards[previousIndex].selected) cards[previousIndex].anim.SetTrigger(animationDeselectTrigger);
        cards[selectedCardIndex].anim.SetTrigger(animationSelectTrigger);
        cardAnimation.StartAnimation(GetTargetDistance());
        initAmbigous = false;
    }
    private void OnAnimation()
    {
        cardHolder.transform.position = Vector2.MoveTowards(cardHolder.transform.position, cardAnimation.TargetDistance, 30f * Time.deltaTime);
        if((Vector2)cardHolder.transform.position == cardAnimation.TargetDistance) 
        {
            cardAnimation.StopAnimation();
            OnAnimationEnd();
        }
    }
    private void OnAnimationEnd()
    {
        
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
            Charm databaseCharm = Instantiate(collectableItem.Charm.GetActiveCharm());
            usedCardIndeces.Add(index);
            ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(databaseCharm.Display.Rarity);
            cards[i].SetHeader(databaseCharm.Display.Description.Name, rarityInfo.color, databaseCharm.Display.Icon);
            cards[i].SetDescription(databaseCharm.Display.Description.Description);
            cards[i].SetQuality(databaseCharm.Display.Rarity);
            for(int j = 0; j < databaseCharm.Display.AdditionalFields.Length; j++)
            {
                cards[i].AddField(CharmDisplay.FormatCharmField(databaseCharm.Display.AdditionalFields[j].Text, databaseCharm.GetActiveCharm()), collectableItem.Charm.GetActiveCharm().Display.AdditionalFields[j].GetColor());
            }
            Destroy(databaseCharm);
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
    internal struct CardAnimation
    {
        private bool inAnimation;
        public Vector2 TargetDistance;
        public void StartAnimation(Vector2 targetDistance)
        {
            TargetDistance = targetDistance;
            inAnimation = true;
        }
        public void StopAnimation() => inAnimation = false;
        public bool InAnimation() => inAnimation;
    }
}
