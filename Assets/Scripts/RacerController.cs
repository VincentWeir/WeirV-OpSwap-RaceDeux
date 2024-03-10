using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RacerController : MonoBehaviour
{
    public float accelerationForce = 15f;
    public float brakingForce = 5f;
    public float maxSpeed = 15f;
    public float reverseSpeed = 10f;
    public float steeringTorque = 40f;

    public int lapNumber = 1;
    public TMP_Text lapCountText;

    public LapTimeRecorder lapTimeRecorder;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
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

    private void OnTriggerEnter(Collider other)
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
            if (lapNumber < 4)
            {
                lapCountText.text = "Lap: " + lapNumber + " / 3";
            }
            else if (lapNumber == 4)
            {
                lapCountText.text = "Lap: 3 / 3";
            }
        }
    }
}