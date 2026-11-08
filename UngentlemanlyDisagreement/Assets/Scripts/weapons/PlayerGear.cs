using System.Collections.Generic;
using UnityEngine;

public class PlayerGear : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;

    [Header("Gear Settings")]
    [SerializeField] private int startingSlots = 2;

    [Header("Weapon Positions")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform dropPoint;

    [Header("Dropped Weapons")]
    [SerializeField] private float dropLifetime = 7.5f;
    [SerializeField] private float dropForwardForce = 2f;
    [SerializeField] private float dropUpwardForce = 2f;

    private class GearItem
    {
        public WeaponDefinition weapon;
        public int usesRemaining;

        public GearItem(WeaponDefinition definition)
        {
            weapon = definition;
            usesRemaining = definition.uses;
        }
    }

    private readonly List<GearItem> gearSlots =
        new List<GearItem>();

    private int selectedSlot;
    private float previousDpadX;
    private GameObject heldModel;

    public int SelectedSlot => selectedSlot;
    public int SlotCount => gearSlots.Count;

    public void SetControllerSlot(int slot)
    {
        playerNumber = slot;
    }

    private void Awake()
    {
        for (int i = 0; i < startingSlots; i++)
            gearSlots.Add(null);
    }

    private void Start()
    {
        RefreshHeldModel();
    }

    private void Update()
    {
        HandleSlotSwitching();
        HandlePickupInput();
    }

    private void HandleSlotSwitching()
    {
        float dpadX = Input.GetAxisRaw(
            "DPadHorizontal_P" + playerNumber);

        if (dpadX < -0.5f && previousDpadX >= -0.5f)
            SelectPreviousSlot();
        else if (dpadX > 0.5f && previousDpadX <= 0.5f)
            SelectNextSlot();

        previousDpadX = dpadX;
    }

    private void HandlePickupInput()
    {
        KeyCode pickupButton = (KeyCode)(
            (int)KeyCode.Joystick1Button2
            + (playerNumber - 1) * 20);

        if (!Input.GetKeyDown(pickupButton))
            return;

        WeaponSpawner nearest = FindNearestAvailableSpawner();

        if (nearest != null)
            nearest.TryPickup(this);
    }

    private WeaponSpawner FindNearestAvailableSpawner()
    {
        WeaponSpawner[] spawners =
            FindObjectsByType<WeaponSpawner>(
                FindObjectsSortMode.None);

        WeaponSpawner nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (WeaponSpawner spawner in spawners)
        {
            if (!spawner.IsAvailable)
                continue;

            float distance = Vector3.Distance(
                transform.position, spawner.transform.position);

            if (distance <= spawner.PickupRange &&
                distance < nearestDistance)
            {
                nearest = spawner;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    public bool HasOpenSlot()
    {
        foreach (GearItem item in gearSlots)
        {
            if (item == null)
                return true;
        }

        return false;
    }

    public bool StoreWeapon(WeaponDefinition weapon)
    {
        if (weapon == null)
            return false;

        // Never overwrite an occupied slot.
        for (int i = 0; i < gearSlots.Count; i++)
        {
            if (gearSlots[i] != null)
                continue;

            gearSlots[i] = new GearItem(weapon);
            selectedSlot = i;
            RefreshHeldModel();

            Debug.Log(
                name + " stored " + weapon.weaponName +
                " in slot " + (i + 1));

            return true;
        }

        Debug.Log(name + " has no empty gear slots.");
        return false;
    }

    public WeaponDefinition GetSelectedWeapon()
    {
        if (selectedSlot < 0 || selectedSlot >= gearSlots.Count)
            return null;

        return gearSlots[selectedSlot]?.weapon;
    }

    public WeaponDefinition GetWeaponInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= gearSlots.Count)
            return null;

        return gearSlots[slotIndex]?.weapon;
    }

    public void SelectNextSlot()
    {
        if (gearSlots.Count == 0)
            return;

        selectedSlot = (selectedSlot + 1) % gearSlots.Count;
        RefreshHeldModel();
        LogSelectedSlot();
    }

    public void SelectPreviousSlot()
    {
        if (gearSlots.Count == 0)
            return;

        selectedSlot =
            (selectedSlot - 1 + gearSlots.Count) % gearSlots.Count;

        RefreshHeldModel();
        LogSelectedSlot();
    }

    public void AddGearSlot()
    {
        gearSlots.Add(null);

        Debug.Log(
            name + " gained a gear slot! Total: " +
            gearSlots.Count);
    }

    private void RefreshHeldModel()
    {
        if (heldModel != null)
            Destroy(heldModel);

        heldModel = null;

        WeaponDefinition weapon = GetSelectedWeapon();

        if (weapon == null || weapon.heldPrefab == null ||
            holdPoint == null)
            return;

        heldModel = Instantiate(weapon.heldPrefab, holdPoint);
        heldModel.transform.localPosition = Vector3.zero;
        heldModel.transform.localRotation = Quaternion.identity;
    }

    public bool HasActiveWeapon()
    {
        return GetSelectedWeapon() != null;
    }

    public void UseSelectedWeapon()
    {
        GearItem item = gearSlots[selectedSlot];

        if (item == null || item.weapon == null)
            return;

        WeaponDefinition weapon = item.weapon;

        switch (weapon.weaponType)
        {
            case WeaponType.Sword:
                UseSword(weapon);
                break;

            case WeaponType.DuelingPistol:
                UsePistol(weapon);
                break;

            case WeaponType.BearTrap:
                ThrowTrap(weapon);
                break;
        }

        item.usesRemaining--;

        if (item.usesRemaining <= 0)
        {
            bool wasTrap = weapon.weaponType == WeaponType.BearTrap;
            DiscardSelectedWeapon(wasTrap);
        }
    }

    private void UseSword(WeaponDefinition weapon)
    {
        if (attackPoint == null)
        {
            Debug.LogWarning(name + ": Attack Point is missing.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position, weapon.attackRange);

        HashSet<PlayerHealth> hitPlayers = new HashSet<PlayerHealth>();

        foreach (Collider hit in hits)
        {
            PlayerHealth target =
                hit.GetComponentInParent<PlayerHealth>();

            if (target == null ||
                target.gameObject == gameObject ||
                !hitPlayers.Add(target))
                continue;

            Vector3 direction =
                target.transform.position - transform.position;

            direction.y = 0f;
            target.TakeDamage(weapon.damage, direction);
        }
    }

    private void UsePistol(WeaponDefinition weapon)
    {
        if (attackPoint == null || weapon.bulletPrefab == null)
        {
            Debug.LogWarning(name + ": Pistol bullet setup is missing.");
            return;
        }

        GameObject bullet = Instantiate(
            weapon.bulletPrefab,
            attackPoint.position,
            attackPoint.rotation);

        Bullet projectile = bullet.GetComponent<Bullet>();

        if (projectile != null)
        {
            projectile.Initialize(
                weapon.damage, gameObject, weapon.bulletSpeed);
        }
        else
        {
            Debug.LogError("Bullet prefab needs a Bullet component.");
            Destroy(bullet);
        }
    }

    private void ThrowTrap(WeaponDefinition weapon)
    {
        if (dropPoint == null || weapon.trapPrefab == null)
        {
            Debug.LogWarning(name + ": Trap setup is missing.");
            return;
        }

        GameObject trap = Instantiate(
            weapon.trapPrefab,
            dropPoint.position,
            dropPoint.rotation);

        BearTrap trapScript = trap.GetComponent<BearTrap>();

        if (trapScript != null)
            trapScript.Initialize(weapon, gameObject);

        Rigidbody trapRb = trap.GetComponent<Rigidbody>();

        if (trapRb != null)
        {
            trapRb.linearVelocity =
                transform.forward * weapon.throwForce
                + Vector3.up * weapon.throwUpwardForce;
        }
    }

    private void DiscardSelectedWeapon(bool wasTrapThrown)
    {
        GearItem item = gearSlots[selectedSlot];

        if (item == null)
            return;

        WeaponDefinition weapon = item.weapon;

        if (heldModel != null)
            Destroy(heldModel);

        heldModel = null;
        gearSlots[selectedSlot] = null;

        // A thrown trap stays in the world instead of being dropped.
        if (!wasTrapThrown && weapon.worldPrefab != null)
        {
            Vector3 position = dropPoint != null
                ? dropPoint.position
                : transform.position + transform.forward;

            GameObject dropped = Instantiate(
                weapon.worldPrefab, position, transform.rotation);

            foreach (Collider col in
                     dropped.GetComponentsInChildren<Collider>())
                col.enabled = true;

            Rigidbody droppedRb = dropped.GetComponent<Rigidbody>();

            if (droppedRb != null)
            {
                droppedRb.isKinematic = false;
                droppedRb.AddForce(
                    transform.forward * dropForwardForce
                    + Vector3.up * dropUpwardForce,
                    ForceMode.Impulse);
            }

            Destroy(dropped, dropLifetime);
        }

        RefreshHeldModel();

        Debug.Log(name + " discarded " + weapon.weaponName);
    }

    private void LogSelectedSlot()
    {
        WeaponDefinition weapon = GetSelectedWeapon();

        Debug.Log(
            name + " selected slot " + (selectedSlot + 1) + ": " +
            (weapon != null ? weapon.weaponName : "Empty"));
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        WeaponDefinition weapon = GetSelectedWeapon();

        if (weapon == null || weapon.weaponType != WeaponType.Sword)
            return;

        Gizmos.DrawWireSphere(
            attackPoint.position, weapon.attackRange);
    }
}