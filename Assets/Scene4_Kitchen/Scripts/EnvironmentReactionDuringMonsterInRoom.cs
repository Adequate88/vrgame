using UnityEngine;
using System.Collections;

public class EnvironmentReactorDuringMonsterInRoom : MonoBehaviour
{
    public PursuerSequence pursuerSequence;
    public Light roomLight;
    public Light roomLight1;
    public Light roomLight2;
    public Light roomLight3;
    public float dimIntensity = 0.3f;
    public float flickerSpeed = 0.1f;
    private float originalIntensity;
    private bool isFlickering = false;

    void Start()
    {
        if (roomLight != null)
            originalIntensity = roomLight.intensity;
    }

    void Update()
    {
        if (pursuerSequence != null && pursuerSequence.sequenceMonsterInsideRoomActive && !isFlickering)
        {
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker()
    {
        isFlickering = true;

        while (pursuerSequence != null && pursuerSequence.sequenceMonsterInsideRoomActive)
        {
            roomLight.intensity = Random.Range(dimIntensity, originalIntensity);
            roomLight1.intensity = Random.Range(dimIntensity, originalIntensity);
            roomLight2.intensity = Random.Range(dimIntensity, originalIntensity);
            roomLight3.intensity = Random.Range(dimIntensity, originalIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }

        roomLight.intensity = originalIntensity;
        roomLight1.intensity = originalIntensity;
        roomLight2.intensity = originalIntensity;
        roomLight3.intensity = originalIntensity;
        isFlickering = false;
    }
}