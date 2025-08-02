using UnityEngine;

public class HorizontalTrap : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        // Move the trap horizontally
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.x <= -49.4)
        {
            Destroy(gameObject);
        }
    }
}
