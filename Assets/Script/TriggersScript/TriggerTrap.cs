using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("Player"))
        {
            if (objectToActivate != null)
            {
                StartCoroutine(DelayActivate());
            }
        }
    }

    private IEnumerator DelayActivate()
    {
        yield return new WaitForSeconds(0.5f);
        objectToActivate.SetActive(true);
    }
}
