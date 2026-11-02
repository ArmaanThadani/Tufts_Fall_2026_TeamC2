
using System.Collections.Generic;

public static class GameSession
{
    // Ordered by the order players join.
    public static readonly List<int> JoinedControllerSlots =
        new List<int>();

    public static bool TryJoinController(int controllerSlot)
    {
        if (JoinedControllerSlots.Contains(controllerSlot))
            return false;

        if (JoinedControllerSlots.Count >= 4)
            return false;

        JoinedControllerSlots.Add(controllerSlot);
        return true;
    }

    public static bool IsJoined(int controllerSlot)
    {
        return JoinedControllerSlots.Contains(controllerSlot);
    }

    public static void ResetLobby()
    {
        JoinedControllerSlots.Clear();
    }
}
