using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainGame");
        Debug.Log("loaded MainGame");
        SceneManager.LoadScene("Ryan", LoadSceneMode.Additive); 
        Debug.Log("loaded Ryan (powerups)");
    }
}
