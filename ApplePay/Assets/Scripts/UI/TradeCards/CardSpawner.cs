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
    private CardAnimation cardAnimation;
    private void Start()
    {
        CardCount = (byte)Random.Range(minCardCount, maxCardCount + 1);
        cards = new TraderCard[CardCount];
        cardAnimation = new CardAnimation();
        SpawnCards();
        StartCoroutine(InitPosition());
        selectedCardIndex = 0;
    }
    private System.Collections.IEnumerator InitPosition()
    {
        yield return new WaitForEndOfFrame();
        
        Vector3 offset = (cards[0].transform.position - transform.position);
        cardHolder.position -= offset;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) && cardAnimation.InAnimation() == false && (selectedCardIndex < CardCount - 1)) 
        {
            int target = selectedCardIndex + 1;
            
            BeginAnimation(cardHolder.position - (cards[target].transform.position - transform.position), selectedCardIndex, target);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && cardAnimation.InAnimation() == false && (selectedCardIndex > 0))
        {
            int target = selectedCardIndex - 1;
            
            BeginAnimation(cardHolder.position - (cards[target].transform.position - transform.position), selectedCardIndex, target);
        }
        if(cardAnimation.InAnimation()) OnAnimation();
    }
    private void BeginAnimation(Vector2 targetDistance, int currentIndex, int targetIndex)
    {
        cardAnimation.StartAnimation(targetDistance, currentIndex, targetIndex);
    }
    private void OnAnimation()
    {
        cardHolder.transform.position = Vector2.MoveTowards(cardHolder.transform.position, cardAnimation.TargetDistance, 10f * Time.deltaTime);
        if((Vector2)cardHolder.transform.position == cardAnimation.TargetDistance) 
        {
            OnAnimationEnd();
        }
    }
    private void OnAnimationEnd()
    {
        cardAnimation.StopAnimation();
        selectedCardIndex = cardAnimation.targetCardIndex;
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
            Charm databaseCharm = Instantiate(collectableItem.charm);
            usedCardIndeces.Add(index);
            ItemRarityInfo rarityInfo = ItemRarityExtension.GetRarityInfo(databaseCharm.Display.Rarity);
            cards[i].SetHeader(databaseCharm.Display.Description.Name, rarityInfo.color, databaseCharm.Display.Icon);
            cards[i].SetDescription(databaseCharm.Display.Description.Description);
            cards[i].SetQuality(databaseCharm.Display.Rarity);
            for(int j = 0; j < databaseCharm.Display.AdditionalFields.Length; j++)
            {
                
                cards[i].AddField(CharmDisplay.FormatCharmField(databaseCharm.Display.AdditionalFields[j].Text, databaseCharm.GetActiveCharm()), collectableItem.charm.GetActiveCharm().Display.AdditionalFields[j].GetColor());
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
        public int swappingCardIndex;
        public int targetCardIndex;
        public void StartAnimation(Vector2 targetDistance ,int current, int target)
        {
            swappingCardIndex = current;
            targetCardIndex = target;
            TargetDistance = targetDistance;
            inAnimation = true;
        }
        public void StopAnimation()
        {
            inAnimation = false;
        }
        public bool InAnimation() => inAnimation;
    }
}
