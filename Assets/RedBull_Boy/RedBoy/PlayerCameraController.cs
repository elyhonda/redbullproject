using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerController playerController;

    [Header("Ground Camera Settings")]
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;
    public float mouseSensitivity = 2f;
    public float autoResetDelay = 3f;
    public float resetSpeed = 2f;
    public Vector3 groundOffset = new Vector3(0, 3, -5);

    [Header("Fly Camera Settings")]
    public float flyDistance = 8f;
    public float flyFollowSpeed = 3f;
    public Vector3 flyOffset = new Vector3(0, 2, -8);
    public float tiltAmount = 15f;

    private float currentYaw = 0f;
    private float idleTime = 0f;
    public Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = Camera.main;
        currentYaw = player.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (playerController != null && playerController.IsFlying())
        {
            HandleFlyCamera();
        }
        else
        {
            HandleGroundCamera();
        }
    }

    void HandleGroundCamera()
    {
        // Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        currentYaw += mouseX;
        idleTime = (Mathf.Abs(mouseX) > 0.1f) ? 0f : idleTime + Time.deltaTime;

        // Position
        Vector3 desiredPosition = player.position + Quaternion.Euler(0, currentYaw, 0) * groundOffset;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref velocity, 1f / followSpeed);

        // Look at Player
        cam.transform.LookAt(player.position + Vector3.up * 1.5f);

        // Auto Reset
        if (idleTime > autoResetDelay)
        {
            float targetYaw = player.eulerAngles.y;
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, resetSpeed * Time.deltaTime);
        }
    }

    void HandleFlyCamera()
    {
        // Camera Position
        Vector3 targetPosition = player.position + player.transform.rotation * flyOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * flyFollowSpeed);

        // Look at Player
        cam.transform.LookAt(player.position + Vector3.up * 1.5f);

        // Inclinar Personagem ao mover Mouse
        float mouseX = Input.GetAxis("Mouse X");
        float tilt = Mathf.Clamp(mouseX, -1f, 1f) * tiltAmount;
        player.localRotation = Quaternion.Euler(0, player.eulerAngles.y, -tilt);
    }
}
