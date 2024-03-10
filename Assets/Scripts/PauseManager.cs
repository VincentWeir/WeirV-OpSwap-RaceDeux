using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public RacerController racerController;
    
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (racerController.lapNumber == 4)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        isPaused = true;
        // You can also display a pause menu or overlay here
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game by setting time scale to 1
        isPaused = false;
        // You can hide the pause menu or overlay here if you displayed it
        Debug.Log("Game Resumed");
    }
}
