using UnityEngine;

public class PlayerSafetyState : MonoBehaviour
{
    public static bool isSafe = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isSafe = true;
            Debug.Log("✅ Player entered a safe zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isSafe = false;
            Debug.Log("❌ Player left the safe zone.");
        }
    }
}