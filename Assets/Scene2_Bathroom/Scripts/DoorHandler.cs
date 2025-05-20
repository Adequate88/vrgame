using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class DoorHandler : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public PuzzleController puzzleController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected bool doorOpened = false;

    void Start()
    {
        doorOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    protected void OnTriggerEnter(Collider other)
    {
        // Check if the puzzle is complete and the door is opened
        if (puzzleController.puzzleComplete && other.CompareTag("DoorTrigger") && !doorOpened)
        {
            doorOpened = true; // Prevent multiple triggers
            GoToNextScene();
        }
    }

    public void GoToNextScene()
    {
        StartCoroutine(GoToNextSceneRoutine());
    }
    protected IEnumerator GoToNextSceneRoutine()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        // Get the current scene index and load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within bounds
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
