using UnityEngine;

public class WaterController : MonoBehaviour
{
    // Maximum y position the object can reach
    public float waterVelocity;

    // Reference to a GameObject
    public GameObject roof;
    public GameObject floor;
    public GameObject underWaterProfile;


    public PuzzleController puzzleController;

    public Swimming swimmingController;

    private Camera cam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the initial position of the object
        transform.position = floor.transform.position - 0.5f * Vector3.up;
        cam = Camera.main;
        underWaterProfile.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the y position of the object if it is below the maximum water level
        if (transform.position.y < roof.transform.position.y && puzzleController.puzzleComplete) {
            transform.position += new Vector3(0, waterVelocity * Time.deltaTime, 0);
        }

        if (cam.transform.position.y < transform.position.y)
        {
            underWaterProfile.SetActive(true);
            swimmingController.enabled = true;
        } else
        {
            underWaterProfile.SetActive(false);
            swimmingController.enabled = false;
        }
    }
}
