using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class RacerController : MonoBehaviour
{
    public float accelerationForce = 15f;
    public float brakingForce = 5f;
    public float maxSpeed = 15f;
    public float reverseSpeed = 10f;
    public float steeringTorque = 40f;

    public GameObject pauseMenu;

    [SerializeField] private UnityEvent<int> lapUpdateEvent;

    public int lapNumber = 1;
    public TMP_Text lapCountText;

    public LapTimeRecorder lapTimeRecorder;

    private Rigidbody rb;
    private CharacterController controller;

    // player actions
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction pauseAction;

    private void Awake()
    {
        pauseAction = playerInput.actions["Pause"];
        moveAction = playerInput.actions["Move"];
        pauseManager = gameObject.FindWithTag("GameManager");
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Erin's code that I can't test if it would work but would look something like this

        // car movement
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;

        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }



        if (pauseAction.triggered)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = Cursor.LockMode.None;
            Cursor.visible = true;
            isPaused = true;
            Time.timeScale = 0
        }

        float steeringInput = Input.GetAxis("Horizontal");
        float moveDirection = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        rb.AddTorque(transform.up * steeringInput * moveDirection * steeringTorque, ForceMode.Acceleration);

        if (moveDirection != 0f)
        {
            rb.AddForce(transform.forward * moveDirection * (moveDirection > 0 ? accelerationForce : reverseSpeed), ForceMode.Acceleration);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(-rb.velocity.normalized * brakingForce, ForceMode.Acceleration);
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (lapNumber == 4)
        {
            accelerationForce = 0;
            brakingForce = 0;
            reverseSpeed = 0;
            steeringTorque = 0;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (other.CompareTag("Lap Trigger"))
        {
            lapNumber++;
            UpdateLapCountDisplay();
            Debug.Log(lapNumber);

            if (lapTimeRecorder != null)
            {
                float lapTime = Time.time;
                lapTimeRecorder.RecordLapTime();
            }
            else
            {
                Debug.LogWarning("LapTimeRecorder reference is not assigned in the RacerController script.");
            }
        }
    }

    void UpdateLapCountDisplay()
    {
        if (lapCountText != null)
        {
            switch (lapNumber)
            {
                case 1:
                    lapCountText.text = "lap 1/3";
                    Break;
                case 2:
                    lapCountText.text = "lap 2/3";
                    Break;
                case 3:
                    lapCountText.text = "lap 3/3";
                    Break;
            }
        }
    }
}