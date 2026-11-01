
using System.Collections.Generic;
using UnityEngine;

public class PlayerGear : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int playerNumber = 1;

    [Header("Gear Settings")]
    [SerializeField] private int startingSlots = 2;

    private List<WeaponDefinition> gearSlots =
        new List<WeaponDefinition>();

    private int selectedSlot = 0;
    private float previousDpadX;

    public int SelectedSlot => selectedSlot;
    public int SlotCount => gearSlots.Count;

    private void Awake()
    {
        for (int i = 0; i < startingSlots; i++)
        {
            gearSlots.Add(null);
        }
    }

    private void Update()
    {
        HandleSlotSwitching();
        HandlePickupInput();
    }

    private void HandleSlotSwitching()
    {
        string axisName = "DPadHorizontal_P" + playerNumber;
        float dpadX = Input.GetAxisRaw(axisName);

        if (dpadX < -0.5f && previousDpadX >= -0.5f)
        {
            SelectPreviousSlot();
        }
        else if (dpadX > 0.5f && previousDpadX <= 0.5f)
        {
            SelectNextSlot();
        }

        previousDpadX = dpadX;
    }

    private void HandlePickupInput()
    {
        KeyCode pickupButton = (KeyCode)(
            (int)KeyCode.Joystick1Button2
            + (playerNumber - 1) * 20
        );

        if (Input.GetKeyDown(pickupButton))
        {
            WeaponSpawner nearestSpawner =
                FindNearestAvailableSpawner();

            if (nearestSpawner != null)
            {
                nearestSpawner.TryPickup(this);
            }
        }
    }

    private WeaponSpawner FindNearestAvailableSpawner()
    {
        WeaponSpawner[] spawners =
            FindObjectsByType<WeaponSpawner>(
                FindObjectsSortMode.None
            );

        WeaponSpawner nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (WeaponSpawner spawner in spawners)
        {
            if (!spawner.IsAvailable)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                spawner.transform.position
            );

            if (distance < nearestDistance &&
                distance <= spawner.PickupRange)
            {
                nearest = spawner;
                nearestDistance = distance;
            }
        }

        return nearest;
    }

    public bool StoreWeapon(WeaponDefinition weapon)
    {
        if (weapon == null || gearSlots.Count == 0)
            return false;

        gearSlots[selectedSlot] = weapon;

        Debug.Log(
            gameObject.name + " stored " + weapon.weaponName +
            " in gear slot " + (selectedSlot + 1)
        );

        return true;
    }

    public WeaponDefinition GetSelectedWeapon()
    {
        return gearSlots[selectedSlot];
    }

    public WeaponDefinition GetWeaponInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= gearSlots.Count)
            return null;

        return gearSlots[slotIndex];
    }

    public void SelectNextSlot()
    {
        selectedSlot = (selectedSlot + 1) % gearSlots.Count;
        LogSelectedSlot();
    }

    public void SelectPreviousSlot()
    {
        selectedSlot--;

        if (selectedSlot < 0)
            selectedSlot = gearSlots.Count - 1;

        LogSelectedSlot();
    }

    public void AddGearSlot()
    {
        gearSlots.Add(null);

        Debug.Log(
            gameObject.name + " gained a gear slot! Total: " +
            gearSlots.Count
        );
    }

    private void LogSelectedSlot()
    {
        WeaponDefinition weapon = gearSlots[selectedSlot];

        string weaponName = weapon != null
            ? weapon.weaponName
            : "Empty";

        Debug.Log(
            gameObject.name + " selected slot " +
            (selectedSlot + 1) + ": " + weaponName
        );
    }
}
