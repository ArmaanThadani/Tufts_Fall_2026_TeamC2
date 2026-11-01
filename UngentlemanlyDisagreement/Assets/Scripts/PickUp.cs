
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerNumber = 1;
    public Transform holdPoint;
    public float pickUpRange = 2f;

    [Header("Gear Slots")]
    public int startingSlots = 2;

    private readonly List<Rigidbody> gearSlots =
        new List<Rigidbody>();

    private int selectedSlot;
    private float previousDpadX;

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

        KeyCode pickupButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (playerNumber - 1) * 20
        );

        if (Input.GetKeyDown(pickupButton))
        {
            if (gearSlots[selectedSlot] != null)
                Drop();
            else
                TryPickUp();
        }
    }

    private void HandleSlotSwitching()
    {
        float dpadX = Input.GetAxisRaw(
            "DPadHorizontal_P" + playerNumber
        );

        if (dpadX < -0.5f && previousDpadX >= -0.5f)
            SelectPreviousSlot();

        if (dpadX > 0.5f && previousDpadX <= 0.5f)
            SelectNextSlot();

        previousDpadX = dpadX;
    }

    private void TryPickUp()
    {
        if (holdPoint == null)
        {
            Debug.LogError("Hold Point is not assigned!", this);
            return;
        }

        Collider[] hits = Physics.OverlapSphere(
            holdPoint.position,
            pickUpRange
        );

        Rigidbody closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Pickup"))
                continue;

            Rigidbody item = hit.attachedRigidbody;

            if (item == null)
                continue;

            float distance = Vector3.Distance(
                holdPoint.position,
                item.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = item;
            }
        }

        if (closest == null)
            return;

        StoreItem(closest);
    }

    public bool StoreItem(Rigidbody item)
    {
        if (item == null || gearSlots[selectedSlot] != null)
            return false;

        gearSlots[selectedSlot] = item;

        item.isKinematic = true;
        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Debug.Log(
            name + " stored " + item.name +
            " in slot " + (selectedSlot + 1)
        );

        return true;
    }

    private void Drop()
    {
        Rigidbody item = gearSlots[selectedSlot];

        if (item == null)
            return;

        item.transform.SetParent(null);
        item.isKinematic = false;

        gearSlots[selectedSlot] = null;
    }

    public void SelectNextSlot()
    {
        selectedSlot = (selectedSlot + 1) % gearSlots.Count;
        UpdateHeldItem();
    }

    public void SelectPreviousSlot()
    {
        selectedSlot--;

        if (selectedSlot < 0)
            selectedSlot = gearSlots.Count - 1;

        UpdateHeldItem();
    }

    private void UpdateHeldItem()
    {
        for (int i = 0; i < gearSlots.Count; i++)
        {
            Rigidbody item = gearSlots[i];

            if (item == null)
                continue;

            if (i == selectedSlot)
            {
                item.transform.SetParent(holdPoint);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }
            else
            {
                // Stow inactive gear on the player for now.
                item.transform.SetParent(transform);
                item.transform.localPosition =
                    new Vector3(0f, -100f, 0f);
            }
        }

        Debug.Log("Selected gear slot " + (selectedSlot + 1));
    }

    public void AddGearSlot()
    {
        gearSlots.Add(null);
    }
}
