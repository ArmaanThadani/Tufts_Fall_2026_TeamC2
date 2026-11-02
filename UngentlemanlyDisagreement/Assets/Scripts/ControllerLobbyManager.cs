
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControllerLobbyManager : MonoBehaviour
{
    [Header("Lobby UI")]
    [SerializeField] private TMP_Text[] playerLabels;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        GameSession.ResetLobby();
        RefreshLobby();
    }

    private void Update()
    {
        string[] controllers = Input.GetJoystickNames();

        for (int slot = 1; slot <= 4; slot++)
        {
            if (slot > controllers.Length ||
                string.IsNullOrEmpty(controllers[slot - 1]))
                continue;

            if (GameSession.IsJoined(slot))
                continue;

            for (int button = 0; button < 20; button++)
            {
                KeyCode key = (KeyCode)(
                    (int)KeyCode.Joystick1Button0 +
                    (slot - 1) * 20 + button
                );

                if (Input.GetKeyDown(key))
                {
                    if (GameSession.TryJoinController(slot))
                        RefreshLobby();

                    break;
                }
            }
        }
    }

    private void RefreshLobby()
    {
        for (int i = 0; i < playerLabels.Length; i++)
        {
            if (i < GameSession.JoinedControllerSlots.Count)
            {
                int slot =
                    GameSession.JoinedControllerSlots[i];

                playerLabels[i].text =
                    "Player " + (i + 1) +
                    " - Controller " + slot;
            }
            else
            {
                playerLabels[i].text =
                    "Player " + (i + 1) +
                    " - Press any button to join";
            }
        }

        startButton.interactable =
            GameSession.JoinedControllerSlots.Count > 0;
    }

    public void StartMatch()
    {
        if (GameSession.JoinedControllerSlots.Count == 0)
            return;

        SceneManager.LoadScene("GameRoom");
    }
}
