using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}