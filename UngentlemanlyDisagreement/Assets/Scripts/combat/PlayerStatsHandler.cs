using UnityEngine;
using System.Collections.Generic;

public class PlayerStatsHandler : MonoBehaviour
{
    public List<PlayerStats> playerStats;

    private void Awake() 
    {
        FindAllPlayers();
    }
    public void FindAllPlayers()
    {
        playerStats = new List<PlayerStats>(FindObjectsByType<PlayerStats>(FindObjectsSortMode.None));
        Debug.Log("PlayerStatsHandler found " + playerStats.Count + " players");
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
