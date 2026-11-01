//For the camera. Do not change.
using Unity.Cinemachine;
using UnityEngine;

public class MultiplayerTargetGroupManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float defaultWeight = 1f;
    [SerializeField] private float defaultRadius = 2f;

    private void Awake() 
    {
        if (targetGroup == null) {
            targetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        }

    }

    public void RegisterTarget(Transform target) {
        if (target == null || targetGroup == null) {
            return;
        }

        targetGroup.AddMember(target, defaultWeight, defaultRadius);
    }


    public void UnRegisterTarget(Transform target) {
        if (target == null || targetGroup == null) {
            return;
        }

        targetGroup.RemoveMember(target);
    }

}
