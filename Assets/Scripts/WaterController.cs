using UnityEngine;

public class WaterController : MonoBehaviour
{
    // Maximum y position the object can reach
    public float waterVelocity;

    // Reference to a GameObject
    public GameObject roof;
    public GameObject floor;
    
    public PuzzleController puzzleController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the initial position of the object
        transform.position = floor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the y position of the object if it is below the maximum water level
        if (transform.position.y < roof.transform.position.y && puzzleController.puzzleComplete) {
            transform.position += new Vector3(0, waterVelocity * Time.deltaTime, 0);
        }
    }
}
