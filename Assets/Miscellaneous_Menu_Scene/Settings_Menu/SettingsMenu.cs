using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SettingsMenu : MonoBehaviour
{
    public GameObject tunnelingVignette; 


    public ControllerInputActionManager controllerInputActionManager; // Assign in Inspector

    public void ToggleRotationMode()
    {
        if (controllerInputActionManager != null)
        {
            controllerInputActionManager.smoothTurnEnabled = !controllerInputActionManager.smoothTurnEnabled;
            Debug.Log($"Smooth Turn Enabled is now: {controllerInputActionManager.smoothTurnEnabled}");
        }
        else
        {
            Debug.LogWarning("ControllerInputActionManager is not assigned.");
        }
    }


    public void ToggleMotionSicknessMode()
    {
        if (tunnelingVignette != null)
        {
            bool currentState = tunnelingVignette.activeSelf;
            tunnelingVignette.SetActive(!currentState);
            Debug.Log($"Tunneling Vignette is now {(!currentState ? "ENABLED" : "DISABLED")}.");
        }
        else
        {
            Debug.LogWarning("Tunneling Vignette GameObject is not assigned in the Inspector.");
        }
    }
    
    

    public void SkipTutorial()
    {
        // Replace with the actual gameplay scene name.
        SceneManager.LoadScene("Bedroom");
    }
}
