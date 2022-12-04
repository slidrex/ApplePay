using UnityEngine;
using System.Linq;

public class CardSpawner : MonoBehaviour
{
    private const byte maxCardsCount = 4;
    [SerializeField] private Card[] cards = new Card[maxCardsCount];
    public CharmDatabase charmDatabase;
    public byte[] uniqueIndexes;
    private void Start()
    {
        GetUniqueIndexes();
        SpawnCards();
    }
    private void SpawnCards()
    {
        for (int i = 0; i < maxCardsCount; i++)
        {
            cards[i].SetCardInfo
            (
                charmDatabase.GetItem(uniqueIndexes[i]).charm.Display.Description.Name,
                charmDatabase.GetItem(uniqueIndexes[i]).charm.Display.Description.Description,
                //charmDatabase.GetItem(uniqueIndexes[i]).charm.Display.AdditionalFields,
                charmDatabase.GetItem(uniqueIndexes[i]).charm.Display.Icon
            );
        }
    }
    private void GetUniqueIndexes()
    {
        uniqueIndexes = new byte[maxCardsCount];
        for(byte i = 0; i < maxCardsCount; i++)
        {
            uniqueIndexes[i] = (byte)Random.Range(0, charmDatabase.GetLength());
        }
        
        while(uniqueIndexes.Length != uniqueIndexes.Distinct().Count())
        {
            for (int i = 0; i < maxCardsCount; i++)
            {
                uniqueIndexes[i] = (byte)Random.Range(0, charmDatabase.GetLength());
            }
        }
    }
    public Card GetCard(byte cardNumber)
    {
        return cards[cardNumber];
    }
}
