using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverCanvas;

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f; // Pause the game
        gameOverCanvas.SetActive(true);
    }

    public void RestartRoom()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Miscellaneous_Menu_Scene/Start_Menu/Start Menu"); // Replace with your start scene name
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}