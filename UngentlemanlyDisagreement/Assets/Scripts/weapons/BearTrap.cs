using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BearTrap : MonoBehaviour
{
    [SerializeField] private float despawnAfterEscape = 1f;

    private Rigidbody rb;
    private WeaponDefinition definition;
    private GameObject owner;

    private PlayerMovement trappedMovement;
    private PlayerHealth trappedHealth;
    private int controllerSlot;
    private int escapeCount;

    private bool landed;
    private bool triggered;
    private bool inert;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(
        WeaponDefinition weapon,
        GameObject trapOwner)
    {
        definition = weapon;
        owner = trapOwner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (landed || inert)
            return;

        // The first solid collision settles the trap.
        landed = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!landed || triggered || inert)
            return;

        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        if (health == null || health.IsDead)
            return;

        if (owner != null && health.gameObject == owner)
            return;

        PlayerMovement movement =
            health.GetComponent<PlayerMovement>();

        PlayerControllerAssignment assignment =
            health.GetComponent<PlayerControllerAssignment>();

        if (movement == null || assignment == null)
            return;

        trappedHealth = health;
        trappedMovement = movement;
        controllerSlot = assignment.ControllerSlot;

        triggered = true;

        Vector3 direction =
            health.transform.position - transform.position;

        direction.y = 0f;

        health.TakeDamage(
            definition != null ? definition.damage : 15f,
            direction);

        // A lethal hit should not leave a dead player waiting to escape.
        if (health.IsDead)
        {
            MakeInert();
            return;
        }

        trappedMovement.enabled = false;

        Rigidbody playerRb = health.GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            Vector3 velocity = playerRb.linearVelocity;
            velocity.x = 0f;
            velocity.z = 0f;
            playerRb.linearVelocity = velocity;
        }

        Debug.Log(health.name + " is trapped! Mash Xbox A to escape.");
    }

    private void Update()
    {
        if (!triggered || inert)
            return;

        if (trappedHealth == null || trappedHealth.IsDead)
        {
            MakeInert();
            return;
        }

        KeyCode aButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (controllerSlot - 1) * 20);

        if (Input.GetKeyDown(aButton))
        {
            escapeCount++;

            int requiredPresses = definition != null
                ? Mathf.Max(1, definition.escapePresses)
                : 12;

            if (escapeCount >= requiredPresses)
                FreePlayer();
        }
    }

    private void FreePlayer()
    {
        if (trappedMovement != null)
            trappedMovement.enabled = true;

        Debug.Log("Player escaped the bear trap!");
        MakeInert();
    }

    private void MakeInert()
    {
        if (inert)
            return;

        inert = true;

        if (trappedMovement != null)
            trappedMovement.enabled = true;

        Destroy(gameObject, despawnAfterEscape);
    }
}