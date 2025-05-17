using UnityEngine;

public class CamScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogError("Oops I forgot to add my player to the scene or to set the tag on it");
        }
    }
    void LateUpdate()
    {
        /*if (player != null)
        {
            transform.position = player.transform.position + offset;
        }*/
    }
}