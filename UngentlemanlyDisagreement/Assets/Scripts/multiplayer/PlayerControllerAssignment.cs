
using UnityEngine;

public class PlayerControllerAssignment : MonoBehaviour
{
    public int PlayerNumber { get; private set; }
    public int ControllerSlot { get; private set; }

    public void SetAssignment(int playerNumber, int controllerSlot)
    {
        PlayerNumber = playerNumber;
        ControllerSlot = controllerSlot;

        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (movement != null)
            movement.SetControllerSlot(controllerSlot);
    }
}
