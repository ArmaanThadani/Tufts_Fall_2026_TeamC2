using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button cardButton;

    private void Awake()
    {
        // Automatically grab the Button component on this same GameObject
        cardButton = GetComponent<Button>();
        
        if (cardButton == null)
        {
            Debug.LogError("No Button found!");
        }
    }
    public void Initialize(
        UpgradeCardStats cardData, 
        Action<UpgradeCardStats> onCardClicked)
    {
        cardNameText.text = cardData.cardName;

        descriptionText.text = cardData.description;
        
        cardButton.onClick.RemoveAllListeners();
        
        cardButton.onClick.AddListener(() => {
            cardButton.interactable = false;

            onCardClicked?.Invoke(cardData);
        
        });
    }
}