using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPositionReset : MonoBehaviour
{
    public Transform xrRig;
    public Transform startPosition; 

    void Start()
    {
        if (xrRig != null && startPosition != null)
        {
            xrRig.position = startPosition.position;
            xrRig.rotation = startPosition.rotation;
        }
    }
}