using UnityEngine;


public class SocketEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool attached;
    public GameObject cubeGrabbable;
    public GameObject cubePushButton;

    void Start()
    {
        attached = false;
        cubePushButton.SetActive(false);
        cubePushButton.GetComponent<Renderer>().enabled = false;
    }

    public void SetAttached()
    {
        attached = true;

        cubeGrabbable.SetActive(false);
        cubePushButton.SetActive(true);
        cubePushButton.GetComponent<Renderer>().enabled = true;
    }
}
