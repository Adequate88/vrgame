using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


public class Crawling : MonoBehaviour
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

    // if TrackedDirection dir is None, all the values below are obsolete, to be reset by state machine
    private Vector3 initialMove = new Vector3(0,0,0);

    // latent info per controller
    private Vector3 lastLeftPos = new Vector3(0, 0, 0);
    private Vector3 lastRightPos = new Vector3(0, 0, 0);

    // true if the last positions registered are valid
    private bool validLastPos = false;

    // swipe info
    private const float SwipeThreshold = 0.001f;
    private float requiredSwipeDist = 0.3f;
    private float currentSwipeDist = 0f;

    private Vector3 positionBeforeCrawl;
    private Vector3 positionUnderBed = new Vector3(-3.1749f, 0f, -1.0465f); //new Vector3(6.9563f, 0f, -2.4816f);
    private bool isCrawlingUnderBed = false; // true if positionBeforeCrawl is relevant
    private Vector3 initialScale;

    // external objects
    public GameObject bed;
    public GameObject bedCollider;
    public GameObject moveProvider;

    private Color originalColor;

    // true if player is within the crawling zone
    private bool crawlingPossible = false;
    private bool crawlingPermanentlyDisabled = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bed = GameObject.FindWithTag("Bed");
        moveProvider = GameObject.FindWithTag("Move Provider");
        bedCollider = GameObject.FindWithTag("Bed Collider");

        originalColor = bed.GetComponent<Renderer>().material.color;
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

            bed.GetComponent<Renderer>().material.color = originalColor;

            // reset everything
            SwipeReset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Crawling is possible: " + crawlingPossible);

        // exit if crawling is not allowed
        if (crawlingPermanentlyDisabled | !crawlingPossible) return;

        Debug.Log("Position: " + transform.position);

        // track hands positions one at a time

        // if left is not already tracked and right has not yet been completed, track right
        if ((swipeLeftStatus == MoveStatus.None | swipeLeftStatus == MoveStatus.Completed) & swipeRightStatus != MoveStatus.Completed)
        {
            trackHand(rightDevice, ref lastRightPos, ref swipeRightStatus, false);
        }

        // if right is not already tracked and left has not yet been completed, track left
        if ((swipeRightStatus == MoveStatus.None | swipeRightStatus == MoveStatus.Completed) & swipeLeftStatus != MoveStatus.Completed)
        {
            trackHand(leftDevice, ref lastLeftPos, ref swipeLeftStatus, true);
        }
        
        // check swipe completeness
        if (swipeLeftStatus == MoveStatus.Completed & swipeRightStatus == MoveStatus.Completed)
        {
            Crawl();
        }
        // else do nothing
    }

    private void trackHand(InputDevice handDevice, ref Vector3 lastControllerPos, ref MoveStatus swipeStatus, bool isLeft)
    {
        Debug.Log("enter hand tracking");

        if (handDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 currentControllerPos))
        {
            if (!validLastPos)
            {
                lastControllerPos = currentControllerPos;
                validLastPos = true;    
            }

            float delta = Vector3.Distance(currentControllerPos, lastControllerPos); // previously lastTrackedPos
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
                    //bed.GetComponent<Renderer>().material.color = Color.white;

                    initialMove = currentMove;
                    currentSwipeDist = delta;

                    swipeStatus = MoveStatus.ForwardOngoing;
                    break;

                // 2 (+ 3)
                case MoveStatus.ForwardOngoing:
                    //bed.GetComponent<Renderer>().material.color = isLeft ? Color.black : Color.red; // left is black, right is red

                    if (Vector3.Dot(currentMove, initialMove) > 0) // same direction move tracked + is it enough to complete half of motion?
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

                    if (Vector3.Dot(currentMove, initialMove) < 0) // opposite directions moves tracked
                    {
                        initialMove = currentMove;
                        currentSwipeDist = delta;

                        swipeStatus = MoveStatus.BackwardOngoing;
                    }
                    break;

                // 5 (+ 6)
                case MoveStatus.BackwardOngoing:
                    //bed.GetComponent<Renderer>().material.color = Color.gray;

                    if (Vector3.Dot(currentMove, initialMove) > 0) // same direction as previous move tracked + is it enough to complete motion?
                    {
                        currentSwipeDist += delta; 

                        if (currentSwipeDist > requiredSwipeDist)
                        {
                            Debug.Log("Swipe Detected!");
                            bed.GetComponent<Renderer>().material.color = Color.yellow;

                            swipeStatus = MoveStatus.Completed;
                        }
                    }
                    break;

                case MoveStatus.Completed:
                    //bed.GetComponent<Renderer>().material.color = Color.yellow;
                    //swipeStatus = MoveStatus.None; leave that to the outside function
                    
                    break;  
            }

            lastControllerPos = currentControllerPos;
        }

    }

    private void Crawl()
    {
        Debug.Log("enter crawling");

        // if position already active, go back to this original position and disable it afterwards
        // if no position has been set active, keep track of the current position before crawling
        if (isCrawlingUnderBed)
        {
            // crawling out

            transform.position = positionBeforeCrawl;
            transform.localScale = initialScale;

            // enable locomotion
            moveProvider.GetComponent<DynamicMoveProvider>().enabled = true;

            isCrawlingUnderBed = false;
            crawlingPermanentlyDisabled = true;
        }
        else
        {
            // crawling in 

            positionBeforeCrawl = transform.position;
            transform.position = positionUnderBed;

            initialScale = transform.localScale;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            // disable locomotion
            moveProvider.GetComponent<DynamicMoveProvider>().enabled = false;

            isCrawlingUnderBed = true;

            // disable crawling until cube 6 has been grabbed
            crawlingPossible = false;
        }

        // because changed positions by a great deal, not to ruin the algorithm
        validLastPos = false;
        SwipeReset();
    }


    public void Cube6EnablesCrawling()
    {
        crawlingPossible = true;
    }

    public void Cube6DisablesCrawling()
    {
        crawlingPossible = false;
    }

    private void SwipeReset()
    {
        swipeRightStatus = MoveStatus.None;
        swipeLeftStatus = MoveStatus.None;
    }
}
