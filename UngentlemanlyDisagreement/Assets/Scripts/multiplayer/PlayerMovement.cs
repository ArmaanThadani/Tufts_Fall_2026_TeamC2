
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Keep the player upright.
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        string horizontalAxis = "Horizontal_P" + playerNumber;
        string verticalAxis = "Vertical_P" + playerNumber;

        moveInput.x = Input.GetAxis(horizontalAxis);
        moveInput.y = Input.GetAxis(verticalAxis);

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        // Move while preserving vertical velocity.
        Vector3 velocity = rb.linearVelocity;
        velocity.x = movement.x * moveSpeed;
        velocity.z = movement.z * moveSpeed;
        rb.linearVelocity = velocity;

        // Rotate toward the movement direction.
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
    }
}
