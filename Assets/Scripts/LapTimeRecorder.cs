using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTimeRecorder : MonoBehaviour
{
    private List<float> lapTimes = new List<float>(); // List to store lap times
    public TMP_Text lapTimer; // Reference to TextMeshPro Text element for lap times display
    private float startTime; // Time when the race started

    // Start is called before the first frame update
    void Start()
    {
        StartRace(); // Start the race when the scene begins
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLapTimeDisplay(); // Update lap time display every frame
    }

    // Method to start the race and reset lap times
    public void StartRace()
    {
        startTime = Time.time; // Record start time
        lapTimes.Clear(); // Clear lap times
    }

    public void RecordLapTime()
    {
        float lapTime = Time.time - startTime; // Calculate lap time
        lapTimes.Add(lapTime); // Add lap time to the list
        UpdateLapTimeDisplay(); // Update lap time display
    }

    // Method to update lap time display
    private void UpdateLapTimeDisplay()
    {
        if (lapTimer != null) // Check if the Text element reference is assigned
        {
            float raceTime = Time.time - startTime; // Calculate race time
            // Update lap time display
            if (lapTimes.Count < 3)
            {
                lapTimer.text = "Time: " + FormatRaceTime(raceTime) + "\n\n";
                if (lapTimes.Count >= 1)
                {
                    lapTimer.text += "Lap Times:\n";
                }
            }
            else if (lapTimes.Count >= 3)
            {
                lapTimer.text = "\n\n\nLap Times:\n";
            }
            for (int i = 0; i < lapTimes.Count; i++)
            {
                lapTimer.text += "Lap " + (i + 1) + ": " + FormatLapTime(lapTimes[i]) + "\n";
            }
        }
        else
        {
            Debug.LogWarning("Lap timer Text element is not assigned in the LapTimeRecorder script.");
        }
    }

    // Method to format lap time as minutes, seconds, and milliseconds
    private string FormatLapTime(float lapTime)
    {
        int minutes = Mathf.FloorToInt(lapTime / 60f);
        int seconds = Mathf.FloorToInt(lapTime % 60f);
        int milliseconds = Mathf.FloorToInt((lapTime - Mathf.Floor(lapTime)) * 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    // Method to format race time as minutes, seconds, and milliseconds
    private string FormatRaceTime(float raceTime)
    {
        int minutes = Mathf.FloorToInt(raceTime / 60f);
        int seconds = Mathf.FloorToInt(raceTime % 60f);
        int milliseconds = Mathf.FloorToInt((raceTime - Mathf.Floor(raceTime)) * 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
