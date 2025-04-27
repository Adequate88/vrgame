using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IngredientCollector : MonoBehaviour
{
    public Collider triggerZone;
    public List<string> requiredTags;

    public XRGrabInteractable potGrab;

    public Transform cheeseSnapPoint;
    public Transform eggSnapPoint;
    public Transform milkSnapPoint;

    public GameObject batterCake;

    private HashSet<string> collectedTags = new HashSet<string>();
    private bool isComplete = false;
    
    
    void OnTriggerEnter(Collider other)
    {

        if (isComplete) return;

        if (requiredTags.Contains(other.tag))
        {
            collectedTags.Add(other.tag);

            // Determine correct snap point
            Transform snapPoint = null;
            if (other.CompareTag("Milk"))
            {
                snapPoint = milkSnapPoint;
            }
            else if (other.CompareTag("Egg"))
            {
                snapPoint = eggSnapPoint;
            }
            else if (other.CompareTag("Cheese"))
            {
                snapPoint = cheeseSnapPoint;
            }

            if (snapPoint != null)
            {
                // Snap item to position & rotation
                other.transform.position = snapPoint.position;
                other.transform.rotation = snapPoint.rotation;

                // Reset scale (prevents distortion)
                other.transform.localScale = Vector3.one;

                // Lock it into the pot
                other.transform.SetParent(transform);

                // Disable physics
                Rigidbody rb = other.attachedRigidbody;
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false; // Disables collider
                }

                Collider col = other.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = false;
                }
            }

            // Check if all required ingredients are added
            if (collectedTags.Count == requiredTags.Count)
            {
                EnablePotGrabbing();
            }
        }
    }

    void EnablePotGrabbing()
    {
        isComplete = true;
        potGrab.enabled = true;

    }
    
    public void EnableBatterCake()
    {
        if (batterCake != null)
        {
            // Hide all child ingredients
            foreach (Transform child in transform)
            {
                if (requiredTags.Contains(child.tag))
                {
                    child.gameObject.SetActive(false);
                }
            }
            batterCake.SetActive(true);
        } 
    }

    void Start()
    {
        batterCake.SetActive(false);
        // EnablePotGrabbing(); // remove this line, only there for prototyping
        potGrab.enabled = false; //set to false first
    }
}
