using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBetterFall : MonoBehaviour {

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	Rigidbody rb;

	[SerializeField] private int controllerSlot = 1;
	private bool jumpPressed;
	KeyCode jumpButton;

	void Awake(){
		rb = GetComponent <Rigidbody> ();
	}

	public void SetControllerSlot(int slot)
    {
        controllerSlot = slot;

		// Xbox A button for the assigned controller.
		jumpButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (controllerSlot - 1) * 20
        );
    }

	private void Update()
    {
		// Xbox A button for the assigned controller.
        if (Input.GetKeyDown(jumpButton))
        {
            jumpPressed = true;
        }
	}

      void FixedUpdate(){
            if (rb.linearVelocity.y < 0) {
                  rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            } else if (rb.linearVelocity.y > 0 && !Input.GetKey(jumpButton)){
                  rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
      }
} 