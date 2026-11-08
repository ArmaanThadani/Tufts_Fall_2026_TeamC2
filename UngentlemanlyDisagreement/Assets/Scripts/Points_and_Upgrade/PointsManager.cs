using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] players = new GameObject[4];
    private int pointsRewarded = 1;

    private void Start() {
        for (int i = 0; i < 4; i++) {
            if (players[i] == null) {
                players[i] = GameObject.Find($"Player{i + 1}");
            }
        }

    }

    public void deathPoints(string playerName) {
        Points.player_points[int.Parse(playerName.Replace("Player", "")) - 1] 
            += pointsRewarded;

        pointsRewarded++;  
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
