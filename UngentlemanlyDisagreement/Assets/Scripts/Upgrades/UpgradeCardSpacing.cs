using UnityEngine;
using System.Collections.Generic;

public class UpgradeCardSpacing : MonoBehaviour
{
    public GameObject cardPrefab;
    private Transform cardContainer;

    public float cardWidth = 200f;
    public float cardHeight = 300f;
    public float spacing = 20f; //spacing between cards
    public int maxCardsPerRow = 5;

    private PlayerStatsHandler statsHandler;

    private void Awake() 
    {
        statsHandler = FindFirstObjectByType<PlayerStatsHandler>();

        cardContainer = transform; //own game object's transform
    }
    public void SpawnAndCenterCards(List<UpgradeCardStats> cards)
    {
        foreach (Transform child in cardContainer) 
            Destroy(child.gameObject);

        int totalCards = cards.Count;

        if (totalCards == 0) 
            return;

        //math to make card go into another row when exceeding width
        int totalRows = Mathf.CeilToInt((float)totalCards / maxCardsPerRow);
        float totalBlockHeight = 
            (totalRows * cardHeight) + ((totalRows - 1) * spacing);

        float totalGroupWidth = 
            (totalCards * cardWidth) + ((totalCards - 1) * spacing);
        float startY = totalBlockHeight / 2f - (cardHeight / 2f);

        int cardIndex = 0;

        for (int r = 0; r < totalRows; r++)
        {
            //math to make cards spaced in row
            int cardsInThisRow = Mathf.Min(maxCardsPerRow, totalCards - cardIndex);

            float rowWidth = (cardsInThisRow * cardWidth) + 
                ((cardsInThisRow - 1) * spacing);
            float startX = -rowWidth / 2f + (cardWidth / 2f);

            float yPos = startY - (r * (cardHeight + spacing));

            for (int c = 0; c < cardsInThisRow; c++)
            {
                GameObject cardObj = Instantiate(cardPrefab, cardContainer);
                RectTransform rect = cardObj.GetComponent<RectTransform>();

                float xPos = startX + (c * (cardWidth + spacing));

                rect.anchoredPosition = new Vector2(xPos, yPos);
                rect.sizeDelta = new Vector2(cardWidth, cardHeight);

                //adding data to card
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    //what happens when card gets clicked
                    cardUI.Initialize(cards[cardIndex], (clickedCardData) => {
                        
                        Debug.Log("Player selected card: " + 
                            clickedCardData.cardName);
                        
                        //Apply the upgrade to the player here
                        statsHandler.ApplyUpgradeToPlayer(0, clickedCardData);
                        
                        Debug.Log("I got here!");
                    });
                }

                //Add Card images here
                
                cardIndex++;
            }
        }
    }

    private void OnCardSelected(UpgradeCardStats data)
    {
        Debug.Log("Selected: " + data.cardName);
    }
}
