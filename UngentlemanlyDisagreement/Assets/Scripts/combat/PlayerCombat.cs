
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private int controllerSlot = 1;
    [SerializeField] private float triggerThreshold = 0.5f;

    [Header("Weapon Check")]
    [SerializeField] private Transform weaponHoldPoint;

    [Header("Slap Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float slapRange = 1.2f;
    [SerializeField] private float slapDamage = 5f;
    [SerializeField] private float slapCooldown = 1f;

    private float nextSlapTime;
    private bool triggerWasPressed;

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
        if (Time.time < nextSlapTime)
            return;

        // Don't slap if a weapon is being held.
        if (weaponHoldPoint != null &&
            weaponHoldPoint.childCount > 0)
        {
            return;
        }

        nextSlapTime = Time.time + slapCooldown;
        Slap();
    }

    private void Slap()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning(name + ": Attack Point is not assigned.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            slapRange
        );

        HashSet<PlayerHealth> hitPlayers =
            new HashSet<PlayerHealth>();

        foreach (Collider hit in hits)
        {
            PlayerHealth target =
                hit.GetComponentInParent<PlayerHealth>();

            if (target == null || target.gameObject == gameObject)
                continue;

            if (!hitPlayers.Add(target))
                continue;

            Vector3 direction =
                target.transform.position - transform.position;

            direction.y = 0f;

            target.TakeDamage(slapDamage, direction);

            Debug.Log(name + " slapped " + target.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, slapRange);
    }
}
