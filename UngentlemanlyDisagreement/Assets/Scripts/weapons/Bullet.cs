using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private Rigidbody rb;
    private float damage;
    private GameObject owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(
        float bulletDamage,
        GameObject shooter,
        float speed)
    {
        damage = bulletDamage;
        owner = shooter;

        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null &&
            other.transform.root.gameObject == owner)
            return;

        PlayerHealth target =
            other.GetComponentInParent<PlayerHealth>();

        if (target != null && !target.IsDead)
        {
            Vector3 direction = target.transform.position -
                                (owner != null
                                    ? owner.transform.position
                                    : transform.position);

            direction.y = 0f;
            target.TakeDamage(damage, direction);
        }

        Destroy(gameObject);
    }
}