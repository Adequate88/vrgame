using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Scene1_Tutorial_Entry/Entrance_Tutorial");
    }

    public void NewGameNoTutorial()
    {
        // SceneManager.LoadScene("Bedroom");
        Debug.Log("NewGameNoTutorial starting, uncomment the thingy to make it work");
    }
    
    
    public void LoadGame()
    {
        Debug.Log("Load Game - Feature not implemented yet.");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
