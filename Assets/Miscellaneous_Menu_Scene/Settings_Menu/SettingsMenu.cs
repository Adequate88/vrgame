using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject tunnelingVignette;

    public void ToggleRotationMode()
    {
        SettingsData.smoothTurnEnabled = !SettingsData.smoothTurnEnabled;
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
