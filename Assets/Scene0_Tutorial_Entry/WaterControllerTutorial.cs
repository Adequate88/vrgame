using UnityEngine;

public class WaterControllerTutorial : WaterController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        underWaterProfile.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.transform.position.y < transform.position.y)
        {
            underWaterProfile.SetActive(true);
            swimmingController.enabled = true;
        } else
        {
            underWaterProfile.SetActive(false);
            swimmingController.enabled = false;
        }
    }
}
