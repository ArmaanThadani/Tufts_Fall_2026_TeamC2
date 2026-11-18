using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public string[] scenes = {"GameRoom"}; 
    
    public void nextScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ControllerLobby");
    }
}
