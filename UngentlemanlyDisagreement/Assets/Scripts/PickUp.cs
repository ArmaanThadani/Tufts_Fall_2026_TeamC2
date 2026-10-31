
using UnityEngine;
 
// Put this on the Player.
// Each item needs: a Collider, a Rigidbody, and the tag "Pickup".
public class PickUp : MonoBehaviour
{
    public Transform holdPoint;      // empty object in front of the player
    public float pickUpRange = 2f;   // how close you need to be
    public KeyCode key = KeyCode.E;
 
    Rigidbody held;
 
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (held == null) TryPickUp();
            else Drop();
        }
    }
 
    void TryPickUp()
    {
        // Find the closest "Pickup" item within range
        Collider[] hits = Physics.OverlapSphere(holdPoint.position, pickUpRange);
        float closest = Mathf.Infinity;
 
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Pickup") || hit.attachedRigidbody == null) continue;
 
            float d = Vector3.Distance(holdPoint.position, hit.transform.position);
            if (d < closest)
            {
                closest = d;
                held = hit.attachedRigidbody;
            }
        }
 
        if (held == null) return;
 
        // Stick it to the hold point
        held.isKinematic = true;
        held.transform.SetParent(holdPoint);
        held.transform.localPosition = Vector3.zero;
        held.transform.localRotation = Quaternion.identity;
    }
 
    void Drop()
    {
        held.transform.SetParent(null);
        held.isKinematic = false;   // physics takes over again
        held = null;
    }
}