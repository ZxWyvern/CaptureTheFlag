using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset between the camera and the player
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned in CameraFollow script.");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionally keep the camera's z position fixed if it's a 2D game
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
