using UnityEngine;

public class PipeAi : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.position + Vector3.left * Time.deltaTime;
        if (transform.position.x <= -62.94)
        {
            Destroy(gameObject);
        }
    }
}
