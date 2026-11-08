using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Weapon Pool")]
    [SerializeField] private WeaponDefinition[] possibleWeapons;

    [Header("Spawner Settings")]
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private Transform displayPoint;

    [Header("Display Animation")]
    [SerializeField] private float spinSpeed = 90f;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    private WeaponDefinition currentWeapon;
    private GameObject displayedWeapon;
    private bool isAvailable;
    private float respawnTime;
    private Vector3 displayStartPosition;

    public bool IsAvailable => isAvailable;
    public float PickupRange => pickupRange;

    private void Start()
    {
        Transform point = displayPoint != null
            ? displayPoint
            : transform;

        displayStartPosition = point.position;
        SpawnNextWeapon();
    }

    private void Update()
    {
        if (!isAvailable)
        {
            if (Time.time >= respawnTime)
                SpawnNextWeapon();

            return;
        }

        if (displayedWeapon == null)
            return;

        displayedWeapon.transform.Rotate(
            Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        displayedWeapon.transform.position =
            displayStartPosition
            + Vector3.up *
            (Mathf.Sin(Time.time * floatSpeed) * floatHeight);
    }

    private void SpawnNextWeapon()
    {
        if (possibleWeapons == null || possibleWeapons.Length == 0)
        {
            Debug.LogError(name + " has no weapons assigned!", this);
            return;
        }

        currentWeapon = possibleWeapons[
            Random.Range(0, possibleWeapons.Length)];

        if (currentWeapon == null)
        {
            Debug.LogError(name + " selected an empty weapon entry.", this);
            return;
        }

        Transform point = displayPoint != null
            ? displayPoint
            : transform;

        if (currentWeapon.worldPrefab != null)
        {
            displayedWeapon = Instantiate(
                currentWeapon.worldPrefab,
                point.position,
                point.rotation);

            // The display is decorative; pickup is range-based.
            foreach (Collider col in
                     displayedWeapon.GetComponentsInChildren<Collider>())
                col.enabled = false;

            Rigidbody rb = displayedWeapon.GetComponent<Rigidbody>();

            if (rb != null)
                rb.isKinematic = true;
        }
        else
        {
            displayedWeapon = null;
            Debug.LogWarning(
                currentWeapon.weaponName + " has no world prefab.", this);
        }

        displayStartPosition = point.position;
        isAvailable = true;

        Debug.Log(name + " is ready with " + currentWeapon.weaponName);
    }

    public void TryPickup(PlayerGear player)
    {
        if (!isAvailable || currentWeapon == null || player == null)
            return;

        float distance = Vector3.Distance(
            player.transform.position, transform.position);

        if (distance > pickupRange)
            return;

        // Full inventory: leave the spawner untouched.
        if (!player.HasOpenSlot())
            return;

        // StoreWeapon chooses an empty slot and equips the weapon.
        if (!player.StoreWeapon(currentWeapon))
            return;

        Debug.Log(player.name + " picked up " + currentWeapon.weaponName);

        isAvailable = false;

        if (displayedWeapon != null)
            Destroy(displayedWeapon);

        displayedWeapon = null;
        currentWeapon = null;
        respawnTime = Time.time + cooldown;
    }
}