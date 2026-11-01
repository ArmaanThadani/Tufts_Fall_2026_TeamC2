using UnityEngine;

public class LocalPlayerIdentity : MonoBehaviour
{
    public int PlayerID { get; private set; }

    public void Initialize(int playerID)
    {
        PlayerID = playerID;
        gameObject.name = "Player_" + playerID;
    }
}