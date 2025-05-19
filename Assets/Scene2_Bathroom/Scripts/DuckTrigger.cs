using UnityEngine;

public class DuckTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PuzzleControllerBathroom puzzleController;
    
    private bool duckNotPlacedYet = false;
    
    void Start()
    {
        duckNotPlacedYet = true;    
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (CheckDuck(other) && duckNotPlacedYet)
        {
            puzzleController.HandleCorrectPlacement();
            duckNotPlacedYet = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckDuck(other) && !duckNotPlacedYet)
        {
            puzzleController.HandleRemoval();
            duckNotPlacedYet = true;
        }
    }

    private bool CheckDuck(Collider other)
    {
        string expectedDuckTag = gameObject.tag.Replace("Socket", "Duck");

        return other.CompareTag(expectedDuckTag);
    }
}
