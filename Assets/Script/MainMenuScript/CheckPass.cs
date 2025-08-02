using UnityEngine;

public class CheckPass : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void Start()
    {


        Vector3 newPosisix = transform.position;
        newPosisix.x = Random.Range(60, 62);
        transform.position = newPosisix;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            }
        }
    }
}
