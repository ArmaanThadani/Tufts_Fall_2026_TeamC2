using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class PlayerStatsHandler : MonoBehaviour
{
    public static List<PlayerStats> playerStats = new List<PlayerStats>();

    private void Awake() 
    {
        if (playerStats.Count != 0) {
            return;
        }

        for (int i = 0; i < 4/*GameSession.JoinedControllerSlots.Count*/; i++) 
        {
            playerStats.Add(new PlayerStats());
            playerStats[i].playerNumber = i + 1;
        }

        DontDestroyOnLoad(gameObject); 
    }

    public void ApplyUpgradeToPlayer(int playerIndex, UpgradeCardStats cardData)
    {
        if (playerIndex >= 0 && playerIndex < playerStats.Count)
        {
            playerStats[playerIndex].ApplyUpgrade(cardData);
            Debug.Log($"Applied {cardData.cardName} to Player at index {playerIndex}");
        }
        else
        {
            Debug.LogError("Invalid player index passed to PlayerStatsHandler!");
        }
    }
}
