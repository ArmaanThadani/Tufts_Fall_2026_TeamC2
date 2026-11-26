
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;
    public float moveSpeed;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Controller")]
    [SerializeField] private int controllerSlot = 1;

    [Header("Jump Settings")]
    public float jumpForce;
    [SerializeField] private float groundCheckDistance = 0.15f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private float knockbackTimer;

    private PlayerStats stats;
    
    public void SetControllerSlot(int slot)
    {
        controllerSlot = slot;
    }

    public void ApplyKnockback(Vector3 force, float duration)
    {
        rb.AddForce(force, ForceMode.Impulse);
        knockbackTimer = duration;
    }

    private void Awake()
    {

        stats = PlayerStatsHandler.playerStats[0];//for testing

        jumpForce = stats.jumpForce;
        moveSpeed = stats.moveSpeed;

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;

    }

    private void Update()
    {
        moveInput.x = Input.GetAxis(
            "Horizontal_P" + controllerSlot
        );

        moveInput.y = Input.GetAxis(
            "Vertical_P" + controllerSlot
        );

        #if !(UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_WEBGL)
            moveInput.y = - moveInput.y;
        #endif

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        // Xbox A button for the assigned controller.
        KeyCode jumpButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (controllerSlot - 1) * 20
        );

        if (Input.GetKeyDown(jumpButton))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        Vector3 velocity = rb.linearVelocity;

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            velocity.x = movement.x * moveSpeed;
            velocity.z = movement.z * moveSpeed;
        }

        rb.linearVelocity = velocity;

        if (movement.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            Quaternion newRotation = Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );


            rb.MoveRotation(newRotation);
        }

        if (jumpPressed && IsGrounded())
        {
            velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			//rb.linearVelocity = Vector3.up * jumpForce;
        }

        jumpPressed = false;
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;

        return Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance + 0.05f
        );
    }
}
