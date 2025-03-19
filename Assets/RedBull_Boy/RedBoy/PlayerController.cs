using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpForce = 6f;
    public float flyAcceleration = 4f;
    public float flyDeceleration = 3f;
    public float maxFlySpeed = 10f;
    public float rotationSpeed = 3f;

    [Header("Key Bindings")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode flyKey = KeyCode.F;
    public KeyCode accelerateFlyKey = KeyCode.W; // Ou seta para cima (verificado no Update)

    [Header("Animation Settings")]
    public Animator animator;

    public string speedParam = "Speed";
    public string isRunningParam = "isRunning";
    public string jumpTriggerParam = "Jump";
    public string isFlyingParam = "isFlying";
    public string isGroundedParam = "isGrounded";

    [Header("Camera Settings")]
    public Transform cameraTarget;
    public float cameraSmoothSpeed = 0.125f;
    public Vector3 cameraOffset;

    private Rigidbody rb;
    private bool isRunning = false;
    private bool isGrounded = true;
    private bool isFlying = false;
    private float currentFlySpeed = 0f;
    private Camera mainCamera;

   void Start()
{
    rb = GetComponent<Rigidbody>();
    mainCamera = Camera.main;

    if (rb == null)
    {
        Debug.LogError("Rigidbody não encontrado no Player! Adicione um Rigidbody no GameObject.");
    }
    if (animator == null)
    {
        Debug.LogError("Animator não atribuído!");
    }

    if (rb != null) rb.useGravity = true;

    if (animator != null)
    {
        animator.SetBool(isGroundedParam, true);
        animator.SetBool(isFlyingParam, false);
    }
}


    void Update()
    {
        HandleInput();
        HandleAnimations();
        HandleCamera();
        CheckGround();
        
    }

   
   
   
   
    void HandleInput()
{
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

    // Running check
    isRunning = Input.GetKey(runKey);

    // Walking or Running
    if (move.magnitude >= 0.1f && !isFlying)
    {
        float targetSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDirection = Quaternion.Euler(0, mainCamera != null ? mainCamera.transform.eulerAngles.y : 0, 0) * move;
        transform.position += moveDirection * targetSpeed * Time.deltaTime;
        transform.forward = moveDirection;
    }

    // Jump
    if (Input.GetKeyDown(jumpKey) && isGrounded && !isFlying)
    {
        if (rb != null)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
        if (animator != null)
        {
            animator.SetBool(isGroundedParam, false);
            animator.SetTrigger(jumpTriggerParam);
        }
    }

    // Fly Toggle
    if (Input.GetKeyDown(flyKey))
    {
        isFlying = !isFlying;
        if (rb != null)
            rb.useGravity = !isFlying;

        currentFlySpeed = 0f; // Reset fly speed

        if (animator != null)
            animator.SetBool(isFlyingParam, isFlying);
    }

    // Fly Movement
    if (isFlying)
    {
        // Rotação com o Mouse
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, mouseX, Space.World);
        transform.Rotate(Vector3.right, -mouseY, Space.Self);

        // Aceleração (W ou seta para cima)
        if (Input.GetKey(accelerateFlyKey) || Input.GetKey(KeyCode.UpArrow))
        {
            currentFlySpeed += flyAcceleration * Time.deltaTime;
            currentFlySpeed = Mathf.Clamp(currentFlySpeed, 0, maxFlySpeed);
        }
        else
        {
            currentFlySpeed -= flyDeceleration * Time.deltaTime;
            currentFlySpeed = Mathf.Clamp(currentFlySpeed, 0, maxFlySpeed);
        }

        transform.position += transform.forward * currentFlySpeed * Time.deltaTime;
    }
}



    void HandleAnimations()
    {
        if (animator == null) return;

        if (isFlying) return; // Flying já setado

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float speed = Mathf.Clamp01(new Vector2(moveHorizontal, moveVertical).magnitude);
        animator.SetFloat(speedParam, speed);

        animator.SetBool(isRunningParam, isRunning);
    }

    void HandleCamera()
    {
        // Proteção contra null
        if (cameraTarget != null && mainCamera != null)
        {
            Vector3 desiredPosition = cameraTarget.position + cameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, cameraSmoothSpeed);
            mainCamera.transform.position = smoothedPosition;
            mainCamera.transform.LookAt(cameraTarget);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (animator != null)
                animator.SetBool(isGroundedParam, true);
        }
    }
void CheckGround()
{
    Ray ray = new Ray(transform.position, Vector3.down);
    float distance = 1.1f; // Ajuste conforme altura do personagem

    if (Physics.Raycast(ray, distance))
    {
        if (!isGrounded)
        {
            isGrounded = true;
            if (animator != null)
                animator.SetBool(isGroundedParam, true);
        }
    }
    else
    {
        if (isGrounded)
        {
            isGrounded = false;
            if (animator != null)
                animator.SetBool(isGroundedParam, false);
        }
    }
}

public bool IsFlying()
{
    return isFlying;
}

public float GetCurrentFlySpeed()
{
    return currentFlySpeed;
}

}

