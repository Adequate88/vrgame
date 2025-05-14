using UnityEngine;
using UnityEngine.Events;


public class PushButton : MonoBehaviour
{
    SocketEvent socket1;
    SocketEvent socket6;

    int cubeValue = 0;

    public static int[] notesInOrder = { 0, 0, 0, 0, 0, 0 };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        socket1 = GameObject.FindWithTag("Cube 1 Interactor").GetComponent<SocketEvent>();
        socket6 = GameObject.FindWithTag("Cube 6 Interactor").GetComponent<SocketEvent>();

        gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().enabled = false;

        cubeValue = int.Parse(gameObject.tag);
    }


    void Update()
    {
        gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().enabled = (socket1.attached & socket6.attached);
    }

    // fill in the next empty spot in the table 
    public void UpdateNotesTable()
    {
        for (int i = 0; i < notesInOrder.Length; ++i)
        {
            if (notesInOrder[i] != 0) continue;
            notesInOrder[i] = cubeValue;
            return;
        }
    }


    public static void ResetNotesTable()
    {
        for (int i = 0; i < notesInOrder.Length; ++i)
        {
            notesInOrder[i] = 0;
        }
    }

}
