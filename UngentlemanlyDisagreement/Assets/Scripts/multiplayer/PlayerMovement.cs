
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.15f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal_P" + playerNumber);
        moveInput.y = Input.GetAxis("Vertical_P" + playerNumber);

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        // Xbox A button for the assigned controller.
        KeyCode jumpButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (playerNumber - 1) * 20
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
        velocity.x = movement.x * moveSpeed;
        velocity.z = movement.z * moveSpeed;
        rb.linearVelocity = velocity;

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

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
