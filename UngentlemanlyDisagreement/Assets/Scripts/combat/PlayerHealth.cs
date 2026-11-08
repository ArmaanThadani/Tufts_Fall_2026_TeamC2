
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackUpwardForce = 1.5f;

    private float currentHealth;
    private Rigidbody rb;
    private PlayerDeath death;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        death = GetComponent<PlayerDeath>();
        currentHealth = maxHealth;
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

            if (death != null)
                death.Die(hitDirection);
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }
}
