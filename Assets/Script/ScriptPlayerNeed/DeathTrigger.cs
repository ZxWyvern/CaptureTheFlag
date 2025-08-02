using UnityEngine;

public class Voidscene : MonoBehaviour
{
    public int damageAmount = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent("PlayerHealth") as MonoBehaviour;
            if (playerHealth != null)
            {
                var method = playerHealth.GetType().GetMethod("TakeDamage");
                if (method != null)
                {
                    method.Invoke(playerHealth, new object[] { damageAmount });
                    
                }
            }
        }
    }
}
