using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStats
{
    public int playerNumber;

    public int points = 0;

    [Header("Health")]
    public float maxHealth = 100f;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackUpwardForce = 1.5f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 50f;

    [Header("Punch")]
    public float punchRange = 1.2f;
    public float punchDamage = 5f;
    public float attackCooldown = 1f;

    [Header("Upgrades")]
    public List<UpgradeCardStats> playerUpgrades;

    public void ApplyUpgrade(UpgradeCardStats cardData) 
    {
        playerUpgrades.Add(cardData);

        // 2. Modify the actual stat using a switch statement
        switch (cardData.upgradeType)
        {
            case UpgradeType.PunchDamage:
                punchDamage += cardData.upgradeValue1;
                break;

            case UpgradeType.Speed:
                moveSpeed += cardData.upgradeValue1;
                break;

            case UpgradeType.MaxHealth:
                maxHealth += cardData.upgradeValue1;
                // Optional: If you track current health, you could increase it here too!
                break;

            default:
                Debug.LogWarning("Unhandled upgrade type: " + cardData.upgradeType);
                break;
        }

        Debug.Log($"Upgraded {cardData.cardName}! New {cardData.upgradeType} value: {cardData.upgradeValue1}");
    }


}
