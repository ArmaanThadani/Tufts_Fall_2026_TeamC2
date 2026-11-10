
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private int controllerSlot = 1;
    [SerializeField] private float triggerThreshold = 0.5f;

    [Header("Punch Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float punchRange = 1.2f;
    [SerializeField] private float punchDamage = 5f;
    [SerializeField] private float attackCooldown = 1f;

    private PlayerGear playerGear;

    private float nextAttackTime;
    private bool triggerWasPressed;

    private void Awake()
    {
        playerGear = GetComponent<PlayerGear>();
    }

    public void SetControllerSlot(int slot)
    {
        controllerSlot = slot;
    }

    private void Update()
    {
        float triggerValue = Input.GetAxisRaw(
            "RightTrigger_P" + controllerSlot
        );

        bool triggerPressed = triggerValue > triggerThreshold;

        if (triggerPressed && !triggerWasPressed)
        {
            TryAttack();
        }

        triggerWasPressed = triggerPressed;
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        if (playerGear != null && playerGear.HasActiveWeapon())
            playerGear.UseSelectedWeapon();
        else
            Punch();
    }

    private void Punch()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning(
                name + ": Attack Point is not assigned."
            );
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            punchRange
        );

        HashSet<PlayerHealth> hitPlayers =
            new HashSet<PlayerHealth>();

        foreach (Collider hit in hits)
        {
            PlayerHealth target =
                hit.GetComponentInParent<PlayerHealth>();

            if (target == null || target.gameObject == gameObject || target.IsDead)
                continue;

            Vector3 direction =
                target.transform.position - transform.position;

            direction.y = 0f;

            // Only hit opponents in front of this player.
            if (Vector3.Dot(transform.forward, direction) <= 0f)
                continue;

            if (!hitPlayers.Add(target))
                continue;

            target.TakeDamage(punchDamage, direction);

            Debug.Log(name + " punched " + target.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, punchRange);
    }
}
