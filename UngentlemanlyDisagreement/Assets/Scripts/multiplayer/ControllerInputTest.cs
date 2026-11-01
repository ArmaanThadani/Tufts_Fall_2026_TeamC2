
using UnityEngine;

public class ControllerInputTest : MonoBehaviour
{
    private void Update()
    {
        string[] joysticks = Input.GetJoystickNames();

        for (int i = 0; i < joysticks.Length; i++)
        {
            if (!string.IsNullOrEmpty(joysticks[i]))
            {
                Debug.Log("Controller slot " + (i + 1) + ": " + joysticks[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("Controller 1 button 0 detected!");
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Debug.Log("Controller 1 button 1 detected!");
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            Debug.Log("Default axes: " + horizontal + ", " + vertical);
        }
    }
}
