using UnityEngine;

public class SpawnCube6 : MonoBehaviour
{
    public GameObject cube6Prefab;

    public void Respawn()
    {
        Instantiate(cube6Prefab, transform.position, transform.rotation);
    }
}
