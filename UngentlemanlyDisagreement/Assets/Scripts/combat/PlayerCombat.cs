
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private int controllerSlot = 1;
    [SerializeField] private float triggerThreshold = 0.5f;

    [Header("Slap Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float slapRange = 1.2f;
    [SerializeField] private float slapDamage = 5f;
    [SerializeField] private float slapCooldown = 1f;

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

        // If a weapon is equipped, use it.
        if (playerGear != null && playerGear.HasActiveWeapon())
        {
            playerGear.UseSelectedWeapon();

            nextAttackTime = Time.time + slapCooldown;
            return;
        }

        // Otherwise, perform the basic slap.
        nextAttackTime = Time.time + slapCooldown;
        Slap();
    }

    private void Slap()
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
