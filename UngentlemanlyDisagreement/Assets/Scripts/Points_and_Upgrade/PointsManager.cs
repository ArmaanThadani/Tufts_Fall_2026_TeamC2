using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private int pointsRewarded = 1;

    public void deathPoints(int playerNumber) {
        if (playerNumber <= 0 || playerNumber > PlayerStatsHandler.playerStats.Count) {
            Debug.Log("Invalid Player Number!");
        }
        PlayerStatsHandler.playerStats[playerNumber - 1].points += pointsRewarded;

        pointsRewarded++;  
    }
}
