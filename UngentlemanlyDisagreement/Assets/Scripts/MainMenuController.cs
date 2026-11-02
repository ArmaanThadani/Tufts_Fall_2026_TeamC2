
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("MAIN MENU START BUTTON CLICKED!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("ControllerLobby");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
