using UnityEngine;

public class PlayerInputLegacy : MonoBehaviour
{
    [SerializeField] private int controllerSlot;

    public int ControllerSlot => controllerSlot;

    public void AssignController(int slot)
    {
        controllerSlot = slot;
    }

    public Vector2 MoveInput
    {
        get
        {
            return GamepadBackend.GetMovement(
                controllerSlot
            );
        }
    }

    public bool AttackPressed =>
        GamepadBackend.GetButtonDown(controllerSlot, 1);

    public bool GrabPressed =>
        GamepadBackend.GetButtonDown(controllerSlot, 2);

    public bool JumpPressed =>
        GamepadBackend.GetButtonDown(controllerSlot, 0);

    public bool UseItemPressed =>
        GamepadBackend.GetButtonDown(controllerSlot, 5);

    private void Update()
    {
        //Debug.Log(
        //    "Player " + GetComponent<LocalPlayerIdentity>().PlayerID
        //    + " movement: " + MoveInput
        //);
    }
}