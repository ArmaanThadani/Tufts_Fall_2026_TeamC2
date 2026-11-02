
using UnityEngine;

public class GameRoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        if (playerPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError(
                "Assign a player prefab and spawn points.",
                this
            );
            return;
        }

        for (int i = 0;
             i < GameSession.JoinedControllerSlots.Count;
             i++)
        {
            int controllerSlot =
                GameSession.JoinedControllerSlots[i];

            Transform spawnPoint =
                spawnPoints[i % spawnPoints.Length];

            GameObject player = Instantiate(
                playerPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            PlayerControllerAssignment assignment =
                player.GetComponent<PlayerControllerAssignment>();

            if (assignment != null)
            {
                assignment.SetAssignment(i + 1, controllerSlot);
            }

            Debug.Log(
                "Spawned Player " + (i + 1) +
                " using controller slot " + controllerSlot
            );
        }
    }
}
