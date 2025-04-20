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
    public float stirDuration = 2f; // how long you need to stir to complete

    private bool spoonInZone = false;
    private bool isPotHeld = false;
    private bool isStirringComplete = false;

    private float stirTimer = 0f;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spoonTag))
        {
            spoonInZone = true;
            stirTimer = 0f; // Reset if they enter again
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(spoonTag))
        {
            spoonInZone = false;
            stirTimer = 0f;
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
        stirTimer = 0f;
    }
    
    void Update()
    {
        if (isStirringComplete) return;

        if (isPotHeld && spoonInZone)
        {
            stirTimer += Time.deltaTime;

            if (stirTimer >= stirDuration)
            {
                CompleteStirring();
            }
        }
    }
    void CompleteStirring()
    {
        isStirringComplete = true;
        stirTimer = 0f;

        if (ingredientCollector != null)
        {
            ingredientCollector.EnableBatterCake();
        }

        Debug.Log("Stirring complete! Batter is now ready.");
    }




}