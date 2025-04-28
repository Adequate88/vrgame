using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool puzzleComplete = false;
    
    private int correctPlacements = 0;
    void Start()
    {
        puzzleComplete = false;
        correctPlacements = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }
    
    public void HandleCorrectPlacement()
    {
        correctPlacements++;
        
        if (correctPlacements >= 4) // All ducks placed
        {
            puzzleComplete = true;
            Debug.Log("Puzzle Complete!");
        }
    }

    public void HandleRemoval()
    {
        correctPlacements--;
    }
}
