using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class DoorHandlerKitchen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public FadeScreen fadeScreen;

    private bool cakeInHand = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cakeInHand && transform.position.x > 4.0f)
        {
            // TODO Update this to final scene
            GoToScene("Kitchen_Room_Scene");
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
