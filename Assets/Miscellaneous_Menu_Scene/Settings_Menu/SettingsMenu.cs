using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
public class SettingsMenu : MonoBehaviour
{
    public GameObject tunnelingVignette;
    public ControllerInputActionManager controllerInputActionManager;
    public void Start()
    {
        // Initialize the settings menu based on the current settings
        tunnelingVignette.SetActive(SettingsData.tunnelingVignetteEnabled);
        controllerInputActionManager.smoothTurnEnabled = SettingsData.smoothTurnEnabled;

    }

    public void ToggleRotationMode()
    {
        SettingsData.smoothTurnEnabled = !SettingsData.smoothTurnEnabled;
        controllerInputActionManager.smoothTurnEnabled = SettingsData.smoothTurnEnabled;
        Debug.Log($"Smooth Turn Enabled is now: {SettingsData.smoothTurnEnabled}");
    }

    public void ToggleMotionSicknessMode()
    {
        SettingsData.tunnelingVignetteEnabled = !SettingsData.tunnelingVignetteEnabled;
        if (tunnelingVignette != null)
        {
            tunnelingVignette.SetActive(SettingsData.tunnelingVignetteEnabled);
            Debug.Log($"Tunneling Vignette is now {(SettingsData.tunnelingVignetteEnabled ? "ENABLED" : "DISABLED")}.");
        }
        else
        {
            Debug.LogWarning("Tunneling Vignette GameObject is not assigned in the Inspector.");
        }
    }

    public void SkipTutorial()
    {
        SceneManager.LoadScene("Bedroom");
    }
}
