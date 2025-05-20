using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections.Generic;

public class HideController : MonoBehaviour
{
    // Assign these in the Inspector:
    public Transform hiddenPosition; // Where the player teleports when hiding.
    public Transform exitPosition; // Where the player appears when coming out.
    public Transform xrOrigin; // Das XR Origin/XR Rig (enthält die Kamera und den Player)
    public FadeScreen fadeScreen; // Reference to the script that handles fade in/out.
    public float fadeWaitTime = 1f; // Time to wait for the fade effect (should match fadeDuration in fadeScreen).
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider; // XR Interaction Toolkit teleportation provider
    public AudioSource breathingAudio;
    public AudioSource closetAudio;
    public GameObject HidingSpot;

    private bool playerInRange = false; // Tracks whether the player is in range.
    private bool isHidden = false; // True if the player is currently hidden.
    private bool isProcessing = false; // Prevents multiple activations at once.
    private Color originalColor;

    // Controller position detection
    public Transform headTransform; // Reference to the HMD/camera transform
    public Transform leftControllerTransform; // Reference to left controller
    public Transform rightControllerTransform; // Reference to right controller
    public float hideGestureThreshold = 0.2f; // Distance threshold for controllers near face
    
    private List<InputDevice> controllers = new List<InputDevice>(); // List to store controllers

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalColor = HidingSpot.GetComponent<Renderer>().material.color;
    }

    // Called when a collider enters the trigger area, thus changing the color of the hidingspot
    private void OnTriggerEnter(Collider other)
    {
        // Der Collider mit dem Player-Tag tritt in den Bereich ein
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("🙈 Player in Hiding Range");

            // Color Change
            HidingSpot.GetComponent<Renderer>().material.color = Color.green;

        }
    }

    // Called when a collider exits the trigger area.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("🐵 Player Exited Hiding Range");

            // Color Change
            HidingSpot.GetComponent<Renderer>().material.color = originalColor;
        }
    }

    // Check for Hiding Gesture when nothing is processing and Player is in Range.
    private void Update()
    {
        if (!isProcessing && playerInRange)
        {
            CheckHidingGesture();
        }
        
        

    }

    private void CheckHidingGesture()
    {
        if (leftControllerTransform != null && rightControllerTransform != null && headTransform != null)
        {
            // Calculate distances from controllers to head
            float leftDistance = Vector3.Distance(leftControllerTransform.position, headTransform.position);
            float rightDistance = Vector3.Distance(rightControllerTransform.position, headTransform.position);
            
            // Debug.Log($"camera/head: {headTransform.position} right: {rightControllerTransform.position} left: {leftControllerTransform.position}");
            
            // Check if both controllers are close enough to the head
            bool hideGestureDetected = (leftDistance < hideGestureThreshold && rightDistance < hideGestureThreshold);
            // Debug.Log($"Left Distance: {leftDistance} Right Distance: {rightDistance}");


            if (hideGestureDetected)
            {
                // Debug.Log("Hide Gesture detected!!!");
            }

            // Handle the dynamic hiding behavior
            if (hideGestureDetected && !isHidden && !isProcessing && playerInRange)
            {
                // Player just made the hiding gesture - hide them
                StartCoroutine(HideInSequence());
            }
            else if (!hideGestureDetected && isHidden && !isProcessing)
            {
                // Player moved controllers away from face - unhide them immediately
                StartCoroutine(HideOutSequence());
            }
        }
    }

    // Sequence for hiding (entering the closet).
    IEnumerator HideInSequence()
    {
        isProcessing = true;
        
        // closet sound
        closetAudio.Play();

        // Fade to black.
        fadeScreen.FadeOut(); // This fades from clear to black

        // wait 1 second
        yield return new WaitForSeconds(fadeWaitTime);

        // start breathing
        breathingAudio.loop = true;  // Ensure it loops
        breathingAudio.Play();
        
        // Das XR Origin zum Versteck teleportieren
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest teleportRequest = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest
        {
            destinationPosition = hiddenPosition.position,
            destinationRotation = hiddenPosition.rotation,
            matchOrientation = UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.MatchOrientation.TargetUp
        };
        
        // Teleportation-Anfrage an den Provider senden
        teleportationProvider.QueueTeleportRequest(teleportRequest);
        
        // Kurze Pause, um sicherzustellen, dass die Teleportation durchgeführt wurde
        yield return new WaitForFixedUpdate();
        
        // Bildschirm bleibt schwarz (kein Fade zurück)
        isHidden = true;
        isProcessing = false;
        // Debug.Log("Hide Sequence done");
        // Debug.Log($"HIDE: Target pos: {hiddenPosition.position}, XR Origin is at: {xrOrigin.position}");
    }

    // Sequence for coming out of hiding.
    IEnumerator HideOutSequence()
    {
        isProcessing = true;

        // stop breathing
        breathingAudio.Stop();

        // closet sound
        closetAudio.Play();
        
        // Das XR Origin zur Ausgangsposition teleportieren
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest teleportRequest = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest
        {
            destinationPosition = exitPosition.position,
            destinationRotation = exitPosition.rotation,
            matchOrientation = UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.MatchOrientation.TargetUp
        };
        
        // Teleportation-Anfrage an den Provider senden
        teleportationProvider.QueueTeleportRequest(teleportRequest);
        
        // Kurze Pause, um sicherzustellen, dass die Teleportation durchgeführt wurde
        //yield return new WaitForFixedUpdate();

        // Fade from black to clear.
        fadeScreen.FadeIn(); // This fades from black to clear.
        yield return new WaitForFixedUpdate();
        
        isHidden = false;
        isProcessing = false;
        // Debug.Log("Hide OUT Sequence done");
        // Debug.Log($"EXIT: Target pos: {exitPosition.position}, XR Origin is at: {xrOrigin.position}");
    }
}