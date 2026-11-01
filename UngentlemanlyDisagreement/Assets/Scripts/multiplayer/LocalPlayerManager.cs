using UnityEngine;

public class LocalPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point is not assigned!");
            return;
        }

        GameObject player = Instantiate(
            playerPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log("Spawned player: " + player.name);
    }
}