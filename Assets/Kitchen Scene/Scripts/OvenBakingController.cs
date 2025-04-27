using UnityEngine;

public class OvenBakingController : MonoBehaviour
{
    [Header("References")]
    public GameObject pot;                     // The pot with battercake
    public GameObject bundtCake;               // Final cake object
    public GameObject knob;                    // Stove-KnobLeft1
    public GameObject ovenDoor;                // Stove-FrontDoor

    [Header("Materials")]
    public Material knobActiveMaterial;
    public Material doorBakingMaterial;
    public Material doorFinishedMaterial;

    [Header("Baking Settings")]
    public float requiredKnobAngle = 90f;
    public float bakingTime = 5f;              // Time in seconds

    private bool potInOven = false;
    private bool bakingStarted = false;
    private float bakingTimer = 0f;

    private Renderer knobRenderer;
    private Renderer doorRenderer;

    void Start()
    {
        knobRenderer = knob.GetComponent<Renderer>();
        doorRenderer = ovenDoor.GetComponent<Renderer>();
        bundtCake.SetActive(false);
    }

    void Update()
    {
        if (potInOven && !bakingStarted)
        {
            float knobAngle = knob.transform.localEulerAngles.z;

            // Handle 360 wrap-around
            if (knobAngle > 180) knobAngle -= 360;

            if (knobAngle >= requiredKnobAngle)
            {
                StartBaking();
            }
        }

        if (bakingStarted)
        {
            bakingTimer += Time.deltaTime;

            if (bakingTimer >= bakingTime)
            {
                FinishBaking();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == pot && other.CompareTag("Pot"))
        {
            potInOven = true;
            Debug.Log("Pot placed inside the oven!");
        }
    }

    void StartBaking()
    {
        bakingStarted = true;
        bakingTimer = 0f;

        // Change materials
        knobRenderer.material = knobActiveMaterial;
        doorRenderer.material = doorBakingMaterial;

        Debug.Log("Baking started!");
    }

    void FinishBaking()
    {
        bakingStarted = false;

        // Change oven door material
        doorRenderer.material = doorFinishedMaterial;

        // Swap pot with bundt cake
        pot.SetActive(false);
        bundtCake.SetActive(true);

        Debug.Log("Baking complete! Cake is ready 🎂");
    }
}
