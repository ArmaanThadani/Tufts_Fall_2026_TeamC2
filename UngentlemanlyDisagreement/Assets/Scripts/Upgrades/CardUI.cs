using UnityEngine;
using TMPro; // Required for TextMeshPro

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void Initialize(UpgradeCardStats cardData)
    {
        cardNameText.text = cardData.cardName;

        descriptionText.text = cardData.description;
    }
}