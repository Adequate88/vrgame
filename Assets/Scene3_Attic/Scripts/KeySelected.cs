using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyInteraction : MonoBehaviour
{
    // Create a static variable that can be accessed globally
    public static bool doorOpen = false;

    // Add this new field for the sound
    public AudioSource keyPickupSound;
    
    // Reference to the XR interactable component
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    
    void Start()
    {
        // Get or add the XR Simple Interactable component
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable == null)
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            Debug.Log("Interactable is null, creating interactable...");

            
        // Subscribe to the select event (when user "clicks" on it in VR)
        interactable.selectEntered.AddListener(OnKeySelected);
    }
    
    // This function is called when the key is clicked/selected
    private void OnKeySelected(SelectEnterEventArgs args)
    {
        // Log that the key was selected (for debugging)
        Debug.Log("Key selected! Attempting to play sound...");
        
        // Play the key pickup sound with null check
        if (keyPickupSound != null)
        {
            keyPickupSound.Play();
            Debug.Log("Sound should be playing now!");
        }
        else
        {
            Debug.Log("ERROR: keyPickupSound is null!");
        }

        // Set the global doorOpen variable to true
        doorOpen = true;
        
        // Log that the key was selected (for debugging)
        Debug.Log("Key selected! Door is now unlocked.");
        
        // Make the key disappear
        gameObject.SetActive(false);
    }
}
