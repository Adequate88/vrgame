using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject spawningPoint;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            spawningPoint.GetComponent<SpawnCube6>().Respawn();
            Destroy(gameObject);
        }
    }
}
