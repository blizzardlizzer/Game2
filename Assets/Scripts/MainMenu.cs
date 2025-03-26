using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayGame()); // Start the coroutine
    }

    IEnumerator PlayGame()
    {
        SceneManager.LoadScene("MainGame");
        Debug.Log("loaded MainGame");

        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        SceneManager.LoadScene("Ryan", LoadSceneMode.Additive); 
        Debug.Log("loaded Ryan (powerups)");
    }
}
