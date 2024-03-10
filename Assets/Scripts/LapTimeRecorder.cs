using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTimeRecorder : MonoBehaviour
{
    private List<float> lapTimes = new List<float>();
    public TMP_Text lapTimer;
    private float startTime;

    // Defining a custom event for lap recording
    public delegate void LapRecordedDelegate();
    public static event LapRecordedDelegate OnLapRecorded;

    // rewrote most of the code as is but changed it to use unity events and invoke event

    void Start()
    {
        StartRace();
    }

    void Update()
    {
        UpdateLapTimeDisplay();
    }

    public void StartRace()
    {
        startTime = Time.time;
        lapTimes.Clear();
    }

    public void RecordLapTime()
    {
        float lapTime = Time.time - startTime;
        lapTimes.Add(lapTime);

        // Trigger the lap recorded event
        if (OnLapRecorded != null)
            OnLapRecorded.Invoke();

        UpdateLapTimeDisplay();
    }

    private void UpdateLapTimeDisplay()
    {
        if (lapTimer != null)
        {
            float raceTime = Time.time - startTime;

            lapTimer.text = "Time: " + FormatRaceTime(raceTime) + "\n\n";

            if (lapTimes.Count >= 1)
            {
                lapTimer.text += "Lap Times:\n";
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

    private string FormatLapTime(float lapTime)
    {
        int minutes = Mathf.FloorToInt(lapTime / 60f);
        int seconds = Mathf.FloorToInt(lapTime % 60f);
        int milliseconds = Mathf.FloorToInt((lapTime - Mathf.Floor(lapTime)) * 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    private string FormatRaceTime(float raceTime)
    {
        int minutes = Mathf.FloorToInt(raceTime / 60f);
        int seconds = Mathf.FloorToInt(raceTime % 60f);
        int milliseconds = Mathf.FloorToInt((raceTime - Mathf.Floor(raceTime)) * 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
