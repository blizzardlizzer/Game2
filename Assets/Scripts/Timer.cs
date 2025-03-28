using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI highscoreText;

    private float elapsedTime = 0f;
    private float bestTime = 0f;
    private bool isRunning = false;

    private const string HighscoreKey = "LocalHighscore";

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat(HighscoreKey, 0f);
        UpdateHighscoreText();
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopStopwatch()
    {
        isRunning = false;
        if (elapsedTime > bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat(HighscoreKey, bestTime);
            PlayerPrefs.Save();
            UpdateHighscoreText();
        }
    }

    private void UpdateHighscoreText()
    {
        highscoreText.text = "HIGHSCORE: " + FormatTime(bestTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}
