using Unity.Cinemachine;
using UnityEngine;

public class InputLockController : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float viewportOffset = 0.02f;
    [SerializeField] private int requiredStableFrames  = 3;

    bool isChecking = false;
    private int stableFrames;

    
    private void Start()
    {
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, HandleEnemyCatchPlayer);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, HandleEnemyCatchPlayer);
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    void HandleEnemyCatchPlayer()
    {
        EventBus.Raise(EventType.SetPlayerInputLocked, true);

        if (isChecking){
            return;
        }

        isChecking = true;
        stableFrames = 0;

        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnCameraUpdated(CinemachineBrain brain)
    {
        if (!isChecking || targetObject == null || mainCamera == null)
            return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(targetObject.position);

        float viewportDistanceFromCenter = Vector2.Distance(
            new Vector2(viewportPos.x, viewportPos.y),
            new Vector2(0.5f, 0.5f)
        );

        bool playerIsInFrontOfCamera = viewportPos.z > 0f;
        bool playerIsCentered = playerIsInFrontOfCamera && viewportDistanceFromCenter <= viewportOffset;

        if (playerIsCentered)
        {
            stableFrames++;
        }
        else
        {
            stableFrames = 0;
        }

        if (stableFrames >= requiredStableFrames)
        {
            isChecking = false;
            stableFrames = 0;
            CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
            EventBus.Raise(EventType.SetPlayerInputLocked, false);
        }
    }
}
