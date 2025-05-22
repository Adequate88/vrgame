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
        if (cam.transform.position.y < transform.position.y - 0.1f)
        {
            // Player is fully underwater
            underWaterProfile.SetActive(true);
            swimmingController.enabled = true;
            swimmingController.isAtSurface = false; // Not at the surface
            continuousMovementController.useGravity = false;
        }
        else if (Mathf.Abs(cam.transform.position.y - transform.position.y) <= 0.1f)
        {
            // Player is at the surface
            underWaterProfile.SetActive(false); // Keep underwater effects active
            swimmingController.enabled = true;
            swimmingController.isAtSurface = true; // At the surface
            continuousMovementController.useGravity = false; // Disable continuous movement controller
        }
        else
        {
            // Player is above the water
            underWaterProfile.SetActive(false);
            swimmingController.enabled = false;
            swimmingController.isAtSurface = false; // Not at the surface
            
            continuousMovementController.useGravity = true;
        }
    }
}
