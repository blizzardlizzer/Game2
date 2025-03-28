using TMPro;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;
    public TextMeshProUGUI centerText;

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
        centerText.text = "GAME PAUSED";
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        centerText.text = "";
        Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}