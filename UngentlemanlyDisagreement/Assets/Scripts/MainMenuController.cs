
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ControllerLobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
