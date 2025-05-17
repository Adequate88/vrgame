using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class DoorHandler : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public PuzzleController puzzleController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool doorOpened = false;

    void Start()
    {
        doorOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the puzzle is complete and the door is opened
        if (puzzleController.puzzleComplete && other.CompareTag("DoorTrigger") && !doorOpened)
        {
            doorOpened = true; // Prevent multiple triggers
            GoToScene("AtticRoom");
        }
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(GoToSceneRoutine(sceneName));
    }
    IEnumerator GoToSceneRoutine(string sceneName)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        SceneManager.LoadScene(sceneName);
        
    }
}
