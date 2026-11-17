using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void MainLevelLoader()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Loaded to Main");
    }
    public void LevelEditorOpener()
    {
        SceneManager.LoadScene(4);
        Debug.Log("Loaded to Level Editor");
    }
    public void GameQuitter()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
