using UnityEngine;
using System.Collections;
/// <summary>
/// Gonna try to comment on everything so you guys can see what the code here does, it's a bit
/// of a mess but it works and it's modular
/// </summary>
public class PursuerSequence : MonoBehaviour
{
    // Initializes everything
    public GameObject player; // Attach to the XR origin
    //private PlayerSafetyState safetyState; // This is the state that says hey, we haven't lost yet

    public AudioSource audioSource; // This one is attached to the Monster
    public AudioClip footstepClip; // This one can be anything, based on the material of your room
    public AudioClip knockingClip; // All these are self - explanatory, you can change them to whatever
    public AudioClip doorOpenClip;
    public AudioClip runningstepClip;
    public AudioClip optionalExtraSoundDuringEntry; // to add more customization to the room - e.g., kitchen has pots and pans

    public float minTimeToFootsteps = 20f; // time to wait before footsteps are heard the first time
    public float maxTimeToFootsteps = 40f; 

    public float minTimeToKnocking = 5f; // time to wait BEFORE starting to knock 
    public float maxTimeToKnocking = 7f;
    public float minRepeatDelay = 60f; // The monster enters every (60-120s)
    public float maxRepeatDelay = 120f;

    public float timeAfterKnockingToCheck = 8f;  // Time after knocking for the monster to enter the room and check player's state

    private Vector3 originalPosition;

    [Header("Movement Settings")]
    public Transform monsterTransform; // assigned to the moster transform
    public float wanderRadius = 3f; // How far the monster moves around
    
    [HideInInspector]
    public bool sequenceMonsterInsideRoomActive = false; // This one here is so we can add other scripts to run when the Monster is in the room (e.g. dimming the lights, etc)


    // private bool sequenceStarted = false; //unused

    void Start()
    {
        originalPosition = monsterTransform.position;
        StartCoroutine(StartPursuerSequence());
        //safetyState = player.GetComponent<PlayerSafetyState>();

        //if (safetyState == null)
        //{
            //Debug.LogError("PlayerSafetyState component not found on player object. Make sure it's attached to the player (XR Origin)!");
        //}   
    }

    IEnumerator StartPursuerSequence()
    {
        while (true) // 🔁 Infinite loop, keeps going until the user actually finishes and leaves the scene
        {
            // Waits, then starts footsteps
            float wait1 = Random.Range(minTimeToFootsteps, maxTimeToFootsteps);
            yield return new WaitForSeconds(wait1);
        
            
            audioSource.PlayOneShot(footstepClip);
            Debug.Log("👣 Footsteps heard...");
            
            // Waits until knocking starts
            float wait2 = Random.Range(minTimeToKnocking, maxTimeToKnocking);
            yield return new WaitForSeconds(wait2);
            audioSource.Stop();

            audioSource.PlayOneShot(knockingClip);
            Debug.Log("🚪 Knocking heard...");

            
            yield return new WaitForSeconds(timeAfterKnockingToCheck);
            audioSource.Stop();
        
            // Door opens, check if player safe
            audioSource.PlayOneShot(doorOpenClip);
            yield return new WaitForSeconds(3f);
            CheckIfPlayerIsSafe();

            // 🕒 Wait before starting the next cycle
            float repeatDelay = Random.Range(minRepeatDelay, maxRepeatDelay);
            Debug.Log($"⏳ Waiting {repeatDelay} seconds until next sequence...");
            yield return new WaitForSeconds(repeatDelay);
        }
    }
    IEnumerator WanderDuringSequence(float duration)
        // makes the monster walk around during the entry
    {
        float timerMovingInHouse = 0f;

        while (timerMovingInHouse < duration)
        {
            // MOVE AROUND IN THE X AND Z DIRECTIONS, Y IS 0
            Vector3 randomOffset = new Vector3(
                Random.Range(-wanderRadius, wanderRadius),
                0,
                Random.Range(-wanderRadius, wanderRadius)
            );

            Vector3 targetPosition = originalPosition + randomOffset;

            float moveDuration = Random.Range(0.5f, 1f); // move every 0.5 - 1 seconds
            float moveTimer = 0f;

            Vector3 startPos = monsterTransform.position;

            while (moveTimer < moveDuration && timerMovingInHouse < duration)
            {
                monsterTransform.position = Vector3.Lerp(startPos, targetPosition, moveTimer / moveDuration);
                yield return null;
                moveTimer += Time.deltaTime;
                timerMovingInHouse += Time.deltaTime;
            }
        }

        // Return to original position
        float returnDuration = 1f;
        float returnTimer = 0f;
        Vector3 currentPos = monsterTransform.position;

        while (returnTimer < returnDuration)
        {
            monsterTransform.position = Vector3.Lerp(currentPos, originalPosition, returnTimer / returnDuration);
            returnTimer += Time.deltaTime;
            yield return null;
        }

        monsterTransform.position = originalPosition;
    }


    void CheckIfPlayerIsSafe()
    {
        if (PlayerSafetyState.isSafe)
        {
            Debug.Log("😌 Player is safe while Monster is in room.");
            StartCoroutine(SafeSequence());
        }
        else
        {
            Debug.Log("💀 You were caught. Game Over.");
            // Trigger death sequence here
        }
    }

    IEnumerator SafeSequence()
    // This is the sequence that we run when the monster is INSIDE the house effectively
    {
        sequenceMonsterInsideRoomActive = true;
        float durationWalk = 8f; // how long the monster is walking around inside the house
        float durationClose = 8f; // how long it takes the door closing audio to play
        float durationBeforeExtraSound = 3f; // If we want an extra sound, how long does the Monster have to walk around before it starts playing
        float totalDuration = durationWalk + durationClose; // Total time to monitor safety

        float timer = 0f;
        float checkInterval = 0.2f;
        bool playingDoorSound = false;
        bool playingExtraSound = false;
        StartCoroutine(WanderDuringSequence(durationWalk)); // Monster goofin off in the room

        // Start first sound
        audioSource.PlayOneShot(runningstepClip);

        while (timer < totalDuration)
        {
            // Check player's safety during the sequence
            if (!PlayerSafetyState.isSafe)
            {
                
                Debug.Log("💀 Player left the safe zone during the safe sequence. Game Over.");
                // TODO: Trigger A losing sequence here or something
                sequenceMonsterInsideRoomActive = false;
                yield break; // Exit coroutine immediately
            }

            yield return new WaitForSeconds(checkInterval);
            timer += checkInterval;
            if (timer > durationWalk && !playingDoorSound)
            {
                // When door opens and monster leaves, stop the steps
                audioSource.Stop();
                audioSource.PlayOneShot(doorOpenClip);
                playingDoorSound = true;
                Debug.Log("🔊 Opening Door sound, monster leaving");

            }

            if (timer > durationBeforeExtraSound && !playingExtraSound)
            {
                audioSource.PlayOneShot(optionalExtraSoundDuringEntry);
                playingExtraSound = true;
            }
        }
        audioSource.Stop();
        Debug.Log("🚪 Safe sequence finished, you can come out now");
        sequenceMonsterInsideRoomActive = false;

    }



}
