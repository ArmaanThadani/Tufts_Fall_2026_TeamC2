using UnityEngine;

public static class GamepadBackend
{
    private static readonly KeyCode[] FirstButton =
    {
        KeyCode.Joystick1Button0,
        KeyCode.Joystick2Button0,
        KeyCode.Joystick3Button0,
        KeyCode.Joystick4Button0
    };

    public static bool IsConnected(int slot)
    {
        string[] names = Input.GetJoystickNames();

        // Unity's joystick slots are numbered 1 through 4.
        return slot >= 1 &&
               slot <= 4 &&
               names.Length >= slot &&
               !string.IsNullOrEmpty(names[slot - 1]);
    }

    public static bool GetButtonDown(int slot, int button)
    {
        if (slot < 1 || slot > 4 ||
            button < 0 || button > 19)
        {
            return false;
        }

        KeyCode key = (KeyCode)(
            (int)FirstButton[slot - 1] + button
        );

        return Input.GetKeyDown(key);
    }

    public static bool GetButton(int slot, int button)
    {
        if (slot < 1 || slot > 4 ||
            button < 0 || button > 19)
        {
            return false;
        }

        KeyCode key = (KeyCode)(
            (int)FirstButton[slot - 1] + button
        );

        return Input.GetKey(key);
    }

    public static Vector2 GetMovement(int slot)
    {
        if (slot < 1 || slot > 4)
            return Vector2.zero;

        float horizontal = Input.GetAxisRaw(
            "Horizontal_P" + slot
        );

        float vertical = Input.GetAxisRaw(
            "Vertical_P" + slot
        );

        return Vector2.ClampMagnitude(
            new Vector2(horizontal, vertical), 1f
        );
    }

    public static bool AnyButtonDown(int slot)
    {
        for (int button = 0; button < 20; button++)
        {
            if (GetButtonDown(slot, button))
                return true;
        }

        return false;
    }
}