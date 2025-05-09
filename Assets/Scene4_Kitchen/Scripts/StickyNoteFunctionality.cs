using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickyNoteFunctionality : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        transform.parent = null;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Override what XR might have reset
        rb.useGravity = true;
    }
}