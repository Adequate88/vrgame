using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CandleInteraction : MonoBehaviour
{
    public Light playerPointLight;  // Assign this in the inspector
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;

    void Start()
    {
        // Get or add an XR Grab Interactable component
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable == null)
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Subscribe to the select event
        interactable.selectEntered.AddListener(OnGrabbed);
        
        // Make sure player light starts off
        if (playerPointLight != null)
            playerPointLight.enabled = false;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Enable the player's point light
        if (playerPointLight != null)
            playerPointLight.enabled = true;
            
        // Hide the candle object
        gameObject.SetActive(false);
    }
}