//For Camera. Don't change
//Add this to each player prefab

using UnityEngine;

public class TargetGroupAutoRegister : MonoBehaviour
{
    private MultiplayerTargetGroupManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null) {
            manager = FindFirstObjectByType<MultiplayerTargetGroupManager>();
        }

        if (manager != null) {
            manager.RegisterTarget(transform);
        }
    }

    private void OnDestroy() {
        if (manager != null) {
            manager.UnRegisterTarget(transform);
        }
    }
}
