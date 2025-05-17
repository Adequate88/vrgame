using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PotStirManager : MonoBehaviour
{
    [Header("References")]
    public Collider stirZone; // Trigger collider
    public IngredientCollector ingredientCollector; // Reference to previous script
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable potGrab;

    [Header("Stirring Settings")]
    public string spoonTag = "Spoon";
    public GameObject spoon;


    private bool spoonInZone = false;
    private bool isPotHeld = false;
    private bool isStirringComplete = false;

    
    // Movement of Spoon so that it is stirring inside the thing
    private Vector3 lastSpoonPosition;
    private float stirProgress = 0f;

    public float requiredStirDistance = 5f;   // Total movement needed to complete stirring
    public float movementThreshold = 0.01f;   // Minimum movement to be considered as stirring

    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spoonTag))
        {
            spoonInZone = true;
            lastSpoonPosition = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(spoonTag))
        {
            spoonInZone = false;
        }
    }
    
    void OnEnable()
    {
        potGrab.selectEntered.AddListener(OnPotGrabbed);
        potGrab.selectExited.AddListener(OnPotReleased);
    }

    void OnDisable()
    {
        potGrab.selectEntered.RemoveListener(OnPotGrabbed);
        potGrab.selectExited.RemoveListener(OnPotReleased);
    }

    void OnPotGrabbed(SelectEnterEventArgs args)
    {
        isPotHeld = true;
    }

    void OnPotReleased(SelectExitEventArgs args)
    {
        isPotHeld = false;
    }
    
    void Update()
    {
        if (isStirringComplete) return;

        if (isPotHeld && spoonInZone)
        {
            Vector3 currentSpoonPosition = spoon.transform.position;
            float movement = Vector3.Distance(currentSpoonPosition, lastSpoonPosition);

            if (movement > movementThreshold)
            {
                stirProgress += movement;   // Add actual movement distance
            }

            lastSpoonPosition = currentSpoonPosition;

            if (stirProgress >= requiredStirDistance)
            {
                CompleteStirring();
            }
        }
    }
    void CompleteStirring()
    {
        isStirringComplete = true;

        if (ingredientCollector != null)
        {
            ingredientCollector.EnableBatterCake();
        }

        Debug.Log("Stirring complete! Batter is now ready.");
    }




}