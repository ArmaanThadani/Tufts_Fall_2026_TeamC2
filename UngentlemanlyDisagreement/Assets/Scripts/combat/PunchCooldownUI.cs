using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PunchCooldownUI : MonoBehaviour
{
    public PlayerCombat player;
    public Image circle;
    public TMP_Text playerLabel;

    void Start()
    {
        int number = Mathf.Max(1, player.GetComponent<PlayerControllerAssignment>().PlayerNumber);
        playerLabel.text = "P" + number;
        circle.rectTransform.anchoredPosition = new Vector2(90 + (number - 1) * 150, 90);
    }

    void Update()
    {
        if (player.CanPunch)
            circle.color = Color.green;
        else
            circle.color = Color.red;
    }
}
