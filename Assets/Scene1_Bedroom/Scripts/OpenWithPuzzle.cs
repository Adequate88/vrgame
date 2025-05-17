using UnityEngine;

public class OpenWithPuzzle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject cube1Interactor;
    public GameObject cube6Interactor;

    PushButton notesData;

    int[] currentNotes;

    bool puzzleIsActive = false;

    int[] correctNotes = { 5, 5, 6, 5, 8, 7 };

    // true when all notes in the table are identical to correctNotes
    public bool puzzleCompleted = false;


    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleCompleted) return;

        puzzleIsActive = (cube1Interactor.GetComponent<SocketEvent>().attached & cube6Interactor.GetComponent<SocketEvent>().attached);

        if (!puzzleIsActive) return;

        currentNotes = PushButton.notesInOrder;

        for (int i = 0; i < correctNotes.Length; ++i)
        {
            if (currentNotes[i] != 0 & currentNotes[i] != correctNotes[i])
            {
                PushButton.ResetNotesTable();
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                return;
            } 
            else if (currentNotes[i] == 0)
            {
                if (i == 0)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.black;
                }
                else
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }

                return;
            }
        }

        puzzleCompleted = true;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
