using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField]
    private CinemachineFollow cinemachineFollow;

    [Header("Smoothing Settings")]
    [SerializeField]
    private float smoothSpeed = 3f;

    [SerializeField]
    private bool canChangeCameraDistance;

    private Vector3 cameraTargetOffset;
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than on CameraManager!");
        }

        Instance = this;
    }

    private void Start()
    {
        cameraTargetOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        if (!canChangeCameraDistance)
        {
            return;
        }

        if (isTransitioning)
        {
            UpdateCurrentCameraDistance();
        }
    }

    private void UpdateCurrentCameraDistance()
    {
        Vector3 currentCameraFollowOffset = cinemachineFollow.FollowOffset;

        if (Mathf.Abs(Vector3.Distance(cameraTargetOffset, currentCameraFollowOffset)) > 0.01f)
        {
            cinemachineFollow.FollowOffset = Vector3.Lerp(
                currentCameraFollowOffset,
                cameraTargetOffset,
                Time.deltaTime * smoothSpeed
            );
        }
        else
        {
            cinemachineFollow.FollowOffset = cameraTargetOffset;
            isTransitioning = false;
        }
    }

    public void ChangeCamearaDistance(float distance)
    {
        cameraTargetOffset = new Vector3(cameraTargetOffset.x, distance, cameraTargetOffset.z);
        isTransitioning = true;
    }
}
