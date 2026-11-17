using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PauseMenuScript : MonoBehaviour
{
    [Header("References")]
    public GameObject OpaqueBackground;
    public GameObject ResumeButton;
    public GameObject RestartButton;
    public GameObject MenuButton;
    public GameObject CloseButton;

    void Start()
    {
        OpaqueBackground.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpaqueBackground.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void Resume()
    {
        OpaqueBackground.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Going back to menu.");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game.");
    }
}
