using UnityEngine;
using System.Collections;

public class PursuerSequence : MonoBehaviour
{
    public GameObject player;
    private PlayerSafetyState safetyState;

    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip knockingClip;
    public AudioClip doorOpenClip;
    public AudioClip runningstepClip;

    public float minTimeToFootsteps = 20f;
    public float maxTimeToFootsteps = 40f;

    public float minTimeToKnocking = 5f;
    public float maxTimeToKnocking = 7f;

    public float timeUntilCheck = 8f;  // Time after knocking to check player's state

    private bool sequenceStarted = false;

    void Start()
    {
        StartCoroutine(StartPursuerSequence());
        safetyState = player.GetComponent<PlayerSafetyState>();

    }

  
    IEnumerator StartPursuerSequence()
    {
        while (true) // 🔁 Infinite loop
        {
            float wait1 = Random.Range(minTimeToFootsteps, maxTimeToFootsteps);
            yield return new WaitForSeconds(wait1);

            audioSource.PlayOneShot(footstepClip);
            Debug.Log("👣 Footsteps heard...");

            float wait2 = Random.Range(minTimeToKnocking, maxTimeToKnocking);
            yield return new WaitForSeconds(wait2);
            audioSource.Stop();

            audioSource.PlayOneShot(knockingClip);
            Debug.Log("🚪 Knocking heard...");

            yield return new WaitForSeconds(timeUntilCheck);
            audioSource.Stop();

            audioSource.PlayOneShot(doorOpenClip);
            yield return new WaitForSeconds(3f);

            CheckIfPlayerIsSafe();

            // 🕒 Wait before starting the next cycle
            float repeatDelay = Random.Range(60f, 120f);
            Debug.Log($"⏳ Waiting {repeatDelay} seconds until next sequence...");
            yield return new WaitForSeconds(repeatDelay);
        }
    }

    void CheckIfPlayerIsSafe()
    {
        if (safetyState != null && safetyState.isSafe)
        {
            Debug.Log("😌 Player is safe. You survived... for now.");
            StartCoroutine(SafeSequence());
        }
        else
        {
            Debug.Log("💀 You were caught. Game Over.");
            // Trigger death sequence here
        }
    }

    IEnumerator SafeSequence()
    {
        float durationWalk = 8f;
        float durationClose = 8f;
        float totalDuration = durationWalk + durationClose; // Total time to monitor safety

        float timer = 0f;
        float checkInterval = 0.2f;
        bool playingDoorSound = false;

        // Start first sound
        audioSource.PlayOneShot(runningstepClip);

        while (timer < totalDuration)
        {
            // Check player's safety during the sequence
            if (safetyState == null || !safetyState.isSafe)
            {
                Debug.Log("💀 Player left the safe zone during the safe sequence. Game Over.");
                // TODO: Trigger death sequence here
                yield break; // Exit coroutine immediately
            }

            yield return new WaitForSeconds(checkInterval);
            timer += checkInterval;
            if (timer > durationWalk && !playingDoorSound)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(doorOpenClip);
                playingDoorSound = true;
                Debug.Log("Opening Door sound");

            }
        }
        audioSource.Stop();

        Debug.Log("🚪 Safe sequence finished.");
    }


}
