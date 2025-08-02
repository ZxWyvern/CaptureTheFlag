using UnityEngine;

public class SpawnOnPass : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (prefabToSpawn != null)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = Random.Range(1, 1);
               Instantiate(prefabToSpawn, newPosition, Quaternion.identity);
                Debug.Log("detected");
            }
        }
    }
}
