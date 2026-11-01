
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Weapon Pool")]
    [SerializeField] private WeaponDefinition[] possibleWeapons;

    [Header("Spawner Settings")]
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private Transform displayPoint;

    private WeaponDefinition currentWeapon;
    private GameObject displayedWeapon;
    private bool isAvailable;
    private float respawnTime;

    public bool IsAvailable => isAvailable;
    public float PickupRange => pickupRange;

    private void Start()
    {
        SpawnNextWeapon();
    }

    private void Update()
    {
        if (!isAvailable && Time.time >= respawnTime)
        {
            SpawnNextWeapon();
        }
    }

    private void SpawnNextWeapon()
    {
        if (possibleWeapons == null ||
            possibleWeapons.Length == 0)
        {
            Debug.LogError(
                name + " has no weapons assigned!",
                this
            );
            return;
        }

        // Choose the weapon immediately when the spawner becomes ready.
        currentWeapon = possibleWeapons[
            Random.Range(0, possibleWeapons.Length)
        ];

        isAvailable = true;

        Transform point = displayPoint != null
            ? displayPoint
            : transform;

        if (currentWeapon.worldPrefab != null)
        {
            displayedWeapon = Instantiate(
                currentWeapon.worldPrefab,
                point.position,
                point.rotation
            );
        }

        Debug.Log(
            name + " is ready with " + currentWeapon.weaponName
        );
    }

    public void TryPickup(PlayerGear player)
    {
        if (!isAvailable || currentWeapon == null || player == null)
            return;

        float distance = Vector3.Distance(
            player.transform.position,
            transform.position
        );

        if (distance > pickupRange)
            return;

        // Do not consume the weapon if the inventory cannot store it.
        if (!player.StoreWeapon(currentWeapon))
            return;

        Debug.Log(
            player.name + " picked up " + currentWeapon.weaponName
        );

        isAvailable = false;

        if (displayedWeapon != null)
        {
            Destroy(displayedWeapon);
            displayedWeapon = null;
        }

        currentWeapon = null;

        // The cooldown begins when the weapon is picked up.
        respawnTime = Time.time + cooldown;
    }
}
