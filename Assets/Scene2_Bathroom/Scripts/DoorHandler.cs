using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class DoorHandler : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public PuzzleController puzzleController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleController.puzzleComplete && transform.position.x > 4.0f)
        {
            GoToScene("Temporary");
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
