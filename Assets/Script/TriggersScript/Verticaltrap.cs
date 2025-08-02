using UnityEngine;

public class VerticalTrap : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        // Move the trap horizontally
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
