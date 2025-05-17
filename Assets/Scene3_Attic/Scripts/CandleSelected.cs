using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Candle_Interaction : MonoBehaviour
{
    // References to the candle components
    public GameObject candle;        // CandleLight Parent
    public GameObject playerCrackling;
    public GameObject playerLight;
    
    // Reference to the XR interactable component
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    
    // Create a static variable that can be accessed globally
    public static bool playerHasLight = false;
    
    void Start()
    {
        // Get or add the XR Simple Interactable component
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            Debug.Log("Interactable is null, creating interactable...");
        }
            
        // Subscribe to the select event (when user "clicks" on it in VR)
        interactable.selectEntered.AddListener(OnCandleSelected);
    }
    
    // This function is called when the Candle is clicked/selected
    private void OnCandleSelected(SelectEnterEventArgs args)
    {
        // Log that the candle was selected (for debugging)
        Debug.Log("Candle got selected!");
        
        // Make flame, noise, smoke and light disappear
        if (candle != null) candle.SetActive(false);

        // Make Light and Crackling appear at Player
        if (playerCrackling != null) playerCrackling.SetActive(true);
        if (playerLight != null) playerLight.SetActive(true);
        
        // Set the global variable so other scripts know player has picked up light
        playerHasLight = true;
    }
}