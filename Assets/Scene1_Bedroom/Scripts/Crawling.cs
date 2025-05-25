using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;


public class Crawling : ContinuousMoveProvider
{
    // hand controller devices, used for tracking
    private InputDevice rightDevice;
    private InputDevice leftDevice;

    // when both are true, swipe completed
    private MoveStatus swipeLeftStatus = MoveStatus.None;
    private MoveStatus swipeRightStatus = MoveStatus.None;

    enum MoveStatus
    {
        None,
        ForwardOngoing,
        ForwardCompleted,
        BackwardOngoing,
        Completed
    }

    // latent info per controller
    private Vector3 lastLeftPos = new Vector3(0, 0, 0);
    private Vector3 lastRightPos = new Vector3(0, 0, 0);

    // true if the last positions registered are valid
    private bool validLastPos = false;

    // swipe info
    private const float SwipeThreshold = 0.001f;
    private float requiredSwipeDist = 0.3f;
    private float currentSwipeDist = 0f;

    private bool isCrawlingUnderBed = false; // true if positionBeforeCrawl is relevant
    private Vector3 initialScale;

    // Tag objects
    GameObject bed;
    GameObject bedCollider;
    GameObject moveProvider;

    private Color originalColor;

    // true if player is within the crawling zone
    private bool crawlingPossible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bed = GameObject.FindWithTag("Bed");
        moveProvider = GameObject.FindWithTag("Move Provider");
        bedCollider = GameObject.FindWithTag("Bed Collider");

        originalColor = bed.GetComponent<Renderer>().material.color;

        initialScale = transform.localScale;
    }

    // considering that OnTriggerEnter is only invoked when entering the zone and not everytime the player IS in the zone
    private void OnTriggerEnter(Collider other)
    {
        if (crawlingPossible) return;

        // check if collider is bed
        if (other.CompareTag("Bed"))
        {
            bed.GetComponent<Renderer>().material.color = Color.yellow;

            // enable crawling
            crawlingPossible = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!crawlingPossible) return;

        // exit bed collider
        if (other.CompareTag("Bed"))
        {
            // disable crawling
            crawlingPossible = false;
            isCrawlingUnderBed = false;

            bed.GetComponent<Renderer>().material.color = originalColor;

            // reset scale and enable locomotion
            transform.localScale = initialScale;
            moveProvider.GetComponent<DynamicMoveProvider>().enabled = true;


            // reset everything
            //SwipeReset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // exit if crawling is not allowed
        if (!crawlingPossible) return;

        // track hands positions one at a time
        // if left is not already tracked and right has not yet been completed, track right
        if (swipeLeftStatus == MoveStatus.None | swipeLeftStatus == MoveStatus.Completed)
        {
            // reset left status
            swipeLeftStatus = MoveStatus.None;
            trackHand(rightDevice, ref lastRightPos, ref swipeRightStatus, false);
        }

        // if right is not already tracked and left has not yet been completed, track left
        if (swipeRightStatus == MoveStatus.None | swipeRightStatus == MoveStatus.Completed)
        {
            // reset right status
            swipeRightStatus = MoveStatus.None;
            trackHand(leftDevice, ref lastLeftPos, ref swipeLeftStatus, true);
        }

        // when first crawling, lower vision and remove dynamic move provider
        if (!isCrawlingUnderBed & (swipeRightStatus != MoveStatus.None | swipeLeftStatus != MoveStatus.None))
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            // disable locomotion
            moveProvider.GetComponent<DynamicMoveProvider>().enabled = false;

            isCrawlingUnderBed = true;
        }
    }

    private void trackHand(InputDevice handDevice, ref Vector3 lastControllerPos, ref MoveStatus swipeStatus, bool isLeft)
    {
        if (handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 currentControllerPos))
        {
            if (!validLastPos)
            {
                lastControllerPos = currentControllerPos;
                validLastPos = true;    
            }

            float delta = Vector3.Distance(currentControllerPos, lastControllerPos);
            if (delta <= SwipeThreshold)
            {
                lastControllerPos = currentControllerPos;
                return;
            }

            Vector3 currentMove = currentControllerPos - lastControllerPos;

            switch (swipeStatus)
            {
                // 1
                case MoveStatus.None: // first move tracked

                    if (Vector3.Dot(currentMove, transform.forward) > 0) // same direction as going forward
                    {
                        currentSwipeDist = delta;

                        swipeStatus = MoveStatus.ForwardOngoing;
                    }
                    break;

                // 2 (+ 3)
                case MoveStatus.ForwardOngoing:

                    if (Vector3.Dot(currentMove, transform.forward) > 0) // same direction as going forward + is it enough to complete half of motion?
                    {
                        currentSwipeDist += delta;

                        if (currentSwipeDist > requiredSwipeDist)
                        {
                            Debug.Log("Swipe Detected!");
                            swipeStatus = MoveStatus.ForwardCompleted;
                        }
                    }
                    break;

                // 4
                case MoveStatus.ForwardCompleted:
                    //bed.GetComponent<Renderer>().material.color = Color.blue;

                    if (Vector3.Dot(currentMove, transform.forward) < 0) // opposite directions moves tracked
                    {
                        currentSwipeDist = delta;

                        swipeStatus = MoveStatus.BackwardOngoing;
                    }
                    break;

                // 5 (+ 6)
                case MoveStatus.BackwardOngoing:
                    //bed.GetComponent<Renderer>().material.color = Color.gray;

                    if (Vector3.Dot(currentMove, -(transform.forward)) > 0) // same direction as going backwards + is it enough to complete motion?
                    {
                        currentSwipeDist += delta;

                        MoveForward();

                        if (currentSwipeDist > requiredSwipeDist)
                        {
                            Debug.Log("Swipe Detected!");
                            bed.GetComponent<Renderer>().material.color = Color.yellow;

                            swipeStatus = MoveStatus.Completed;
                        }
                    }
                    break;

                case MoveStatus.Completed:
                    //swipeStatus = MoveStatus.None; leave that to update
                    
                    break;  
            }

            lastControllerPos = currentControllerPos;
        }

    }


    private void MoveForward()
    {
        MoveRig((transform.forward * 1.5f) * Time.deltaTime);
    }
}
