using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door_Interaction : MonoBehaviour
{
    // References to the components
    public GameObject door;        
    public AudioSource lockedDoorAudio;
    public AudioSource unlockedDoorAudio;
    
    // Reference to the XR interactable component
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    
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
        interactable.selectEntered.AddListener(OnDoorSelected);
    }

    // This function is called when the key is clicked/selected
    private void OnDoorSelected(SelectEnterEventArgs args)
    {
        if (KeyInteraction.doorOpen)
        {
            Debug.Log("Door is unlocked!");

            unlockedDoorAudio.Play();
           
        }
        else
        {
            Debug.Log("Door is locked!");

            lockedDoorAudio.Play();

        }
    }

}