using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [Header("Camera Movement Values")]
    public Vector2 sensitivity = new Vector2(2f, 2f);
    public float maxverticalClamp = 40f;
    public float minverticalClamp = 10f;
    
    [Header("Camera Positioning Values")]
    public Vector3 rightshoulderOffset = new Vector3(0.6f, 1.54f, -2.08f); // value tp place camera on the right shoulder of the player
    public Vector3 leftShoulderOffset = new Vector3(0f,0f, 0f); // value to place camera on left shoulder the player

    [SerializeField] private bool useRightShoulder = true; // toggle for switching between right and left shoulder view
    [SerializeField] private float VerticalInput; // Value to check if player is moving backwards or not
    [SerializeField] private float rotBackSpeed = 2f; 

    private float yaw;
    private float pitch;
    public Transform player;

    // Timer for look back at player
    [SerializeField] private float backwardTimer = 0f;
    [SerializeField] private float lookBackDuration = 20f;
    public float MoveforwardDuration = 2f; // duration for which player moves forward when looking back
    private bool isLookingAtBack = false;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        Toggle();
        HandleCamera();

    }

    private void HandleCamera()
    {
        // Mouse input / joystick input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity.x;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minverticalClamp, maxverticalClamp);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0).normalized;

        // selected based on toggle
        Vector3 offset = useRightShoulder ? rightshoulderOffset : leftShoulderOffset;

        // if player is moving backwads ,the camera goes behind the player
        VerticalInput = Input.GetAxis("Vertical");
        if(VerticalInput <= -0.6f) // fully pressed back
        {
            backwardTimer += Time.deltaTime;

            if(backwardTimer >= lookBackDuration) // after 20 secs
            {
                isLookingAtBack = true;
                backwardTimer = 0; // reset timer after looking at back
            }
        }
        else
        {
            backwardTimer = 0f; // reset when not moving back
            isLookingAtBack = false;
        }
        // rotate cam to look at player back smoothly
        if(isLookingAtBack)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.forward,Vector3.up);
            rotation = Quaternion.Slerp(rotation, targetRotation, (Time.deltaTime * rotBackSpeed )/10f); // here dividing speed by make it smooth instead of jittery

            // make player move forward for N seconds 
            player.GetComponent<PlayerMovement>().ForceForwardForSeconds(MoveforwardDuration);

            isLookingAtBack = false; // reset after looking at back
        }
        transform.position = player.position + rotation * offset;
        transform.rotation = rotation;
    }
    private void Toggle()
    {
        if (Input.GetButtonDown("RB"))
        {
            useRightShoulder = true;
        }
        else if (Input.GetButtonDown("LB"))
        {
            useRightShoulder = false;
        }
    }
}
