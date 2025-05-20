using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DoorHandlerFinalRoom : DoorHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzleController.puzzleComplete = true;
    }

    IEnumerator GoToNextSceneRoutine()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        SceneManager.LoadScene(0);
    }
}
