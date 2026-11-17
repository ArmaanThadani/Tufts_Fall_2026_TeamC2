
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHealth : MonoBehaviour
{
    private float maxHealth;
    private float knockbackForce;
    private float knockbackUpwardForce;

    private float currentHealth;
    private Rigidbody rb;
    private PlayerDeath death;
    public bool IsDead { get; private set; }

    [SerializeField] private PointsManager pointsManager;

    private PlayerStats stats;

    private void Awake()
    {
        stats = PlayerStatsHandler.playerStats[0];//for testing
        rb = GetComponent<Rigidbody>();
        death = GetComponent<PlayerDeath>();

        maxHealth = stats.maxHealth;
        knockbackForce = stats.knockbackForce;
        knockbackUpwardForce = stats.knockbackUpwardForce;

        currentHealth = maxHealth;

        if (pointsManager == null)
        {
            pointsManager = FindFirstObjectByType<PointsManager>();
        }
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        if (IsDead || damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        // Apply knockback away from the attacker.
        Vector3 direction = hitDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();

            PlayerMovement movement = GetComponent<PlayerMovement>();

            if (movement != null)
            {
                Vector3 force =
                    direction * knockbackForce
                    + Vector3.up * knockbackUpwardForce;

                movement.ApplyKnockback(force, 0.2f);
            }
        }

        Debug.Log(name + " health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            IsDead = true;

            if (pointsManager != null)
            {
                pointsManager.deathPoints(gameObject.name);
            }
            else
            {
                Debug.LogWarning(
                    "PointsManager not found! Death points were not recorded."
                );
            }

            if (death != null)
            {
                death.Die(hitDirection);
            }
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
}
