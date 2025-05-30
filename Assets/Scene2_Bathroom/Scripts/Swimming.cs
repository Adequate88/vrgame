using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class Swimming : ContinuousMoveProvider
{
    public float distanceThreshold = 0.2f; // Distance threshold for swimming gesture
    public float maxSpeed = 2.5f; // Maximum speed for swimming
    public float waterSurfaceHeight = 5.0f; // Height of the water surface
    public float surfaceDrag = 0.5f; // Drag applied when swimming at the surface
    public AudioSource audioSource; // Reference to the AudioSource component
    private bool controllersTogether = false; // True if controllers are currently together
    private Vector3 currentVelocity = Vector3.zero; // Stores the current velocity for continuous movement
    private float propulsionDecayRate = 1.0f; // Rate at which propulsion slows down over time
    private Vector3 sinkingVelocity = Vector3.down * 0.5f; // Tendency to sink

    public bool isAtSurface = false; // True if the player is at or near the water surface

    [SerializeField]
    XRInputValueReader<Vector3> LeftHandVelocity = new XRInputValueReader<Vector3>("Left Hand Velocity");

    [SerializeField]
    XRInputValueReader<Vector3> RightHandVelocity = new XRInputValueReader<Vector3>("Right Hand Velocity");

    [SerializeField]
    XRInputValueReader<Vector3> LeftHandPosition = new XRInputValueReader<Vector3>("Left Hand Position");

    [SerializeField]
    XRInputValueReader<Vector3> RightHandPosition = new XRInputValueReader<Vector3>("Right Hand Position");

    [SerializeField]
    Transform HeadTransform; // Reference to the HMD/camera transform

    void Update()
    {
        // Check if the player is at or near the surface

        // Get the positions of the left and right controllers
        var leftHandPositionValue = LeftHandPosition.ReadValue();
        var rightHandPositionValue = RightHandPosition.ReadValue();

        // Compute the distance between the controllers
        float controllerDistance = Vector3.Distance(leftHandPositionValue, rightHandPositionValue);
        
        // Check if controllers are together
        if (controllerDistance < distanceThreshold)
        {
            if (!controllersTogether)
            {
                // Controllers just came together
                controllersTogether = true;
                Debug.Log("Controllers are together. Ready to swim.");
            }
        }
        else
        {
            if (controllersTogether)
            {
                // Controllers just spread apart
                controllersTogether = false;
                Debug.Log("Controllers spread apart. Propelling forward.");
                PropelForward(); // Trigger propulsion
                audioSource.Play(); // Play the swimming sound
            }
        }

        // Apply continuous movement based on current velocity
        ApplyContinuousMovement();
    }

    private void PropelForward()
    {
        // Get the velocity of the left and right hand + Direction of the eye gaze
        Vector3 leftHandVelocityValue = LeftHandVelocity.ReadValue();
        Vector3 rightHandVelocityValue = RightHandVelocity.ReadValue();
        Vector3 gazeForward = HeadTransform.forward;

        // Calculate the average speed of the hands
        float leftHandSpeed = leftHandVelocityValue.magnitude;
        float rightHandSpeed = rightHandVelocityValue.magnitude;
        float averageSpeed = (leftHandSpeed + rightHandSpeed) / 2;

        // Set a fixed propulsion speed for testing (also if velocity tracking is broken)
        averageSpeed = 5.0f;

        // Clamp the speed to a maximum
        averageSpeed = Mathf.Clamp(averageSpeed, 0, maxSpeed);

        // Adjust propulsion direction and speed if at the surface
        if (isAtSurface)
        {
            // Reduce upward movement and apply surface drag
            gazeForward.y = Mathf.Min(gazeForward.y, 0); // Prevent upward movement
            averageSpeed *= (1 - surfaceDrag); // Reduce speed at the surface
        }

        // Calculate the propulsion direction
        currentVelocity = gazeForward * averageSpeed;

        Debug.Log($"Propelling forward in direction: {currentVelocity} with speed: {averageSpeed}");
    }

    private void ApplyContinuousMovement()
    {
        // Apply the current velocity to move the rig

        if (currentVelocity.magnitude > 0.01f)
        {
            MoveRig((currentVelocity + sinkingVelocity) * Time.deltaTime);

            // Gradually reduce the velocity over time to simulate drag
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, propulsionDecayRate * Time.deltaTime);
        }
        else
        { 
            MoveRig(sinkingVelocity * Time.deltaTime); // Just sink

        }

        //MoveRig(sinkingVelocity * Time.deltaTime);
    }
}
