using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private readonly List<int> joinedSlots =
        new List<int>();

    private bool matchStarted;

    public int JoinedPlayerCount => joinedSlots.Count;

    private void Update()
    {
        if (matchStarted || joinedSlots.Count >= 4)
            return;

        for (int slot = 1; slot <= 4; slot++)
        {
            if (joinedSlots.Contains(slot))
                continue;

            if (!GamepadBackend.IsConnected(slot))
                continue;

            if (GamepadBackend.AnyButtonDown(slot))
            {
                joinedSlots.Add(slot);

                Debug.Log(
                    "Player " + joinedSlots.Count +
                    " joined using joystick slot " + slot
                );
            }
        }
    }

    public void StartMatch()
    {
        if (matchStarted || joinedSlots.Count == 0)
            return;

        matchStarted = true;

        for (int i = 0; i < joinedSlots.Count; i++)
        {
            int slot = joinedSlots[i];

            Transform spawn = spawnPoints[
                i % spawnPoints.Length
            ];

            GameObject player = Instantiate(
                playerPrefab,
                spawn.position,
                spawn.rotation
            );

            PlayerInputLegacy input =
                player.GetComponent<PlayerInputLegacy>();

            LocalPlayerIdentity identity =
                player.GetComponent<LocalPlayerIdentity>();

            input.AssignController(slot);
            identity.Initialize(i + 1);
        }
    }
}