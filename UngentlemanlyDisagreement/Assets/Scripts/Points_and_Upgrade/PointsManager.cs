using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private int pointsRewarded = 1;


    public void deathPoints(string playerName) {
        Points.player_points[int.Parse(playerName.Replace("Player", "")) - 1] 
            += pointsRewarded;

        pointsRewarded++;  
    }
}
