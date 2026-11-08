
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float deathForce = 3f;
    [SerializeField] private float upwardForce = 1f;

    private Rigidbody rb;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private bool hasDied;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    public void Die(Vector3 hitDirection)
    {
        if (hasDied)
            return;

        hasDied = true;

        if (movement != null)
            movement.enabled = false;

        if (combat != null)
            combat.enabled = false;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;

            Vector3 forceDirection =
                hitDirection.normalized + Vector3.up * upwardForce;

            rb.AddForce(
                forceDirection.normalized * deathForce,
                ForceMode.Impulse
            );
        }

        Debug.Log(name + " has died!");
    }
}
