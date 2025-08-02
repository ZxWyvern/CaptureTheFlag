using UnityEngine;
using UnityEngine.Playables;
public class TriggerCamera : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            playableDirector.Play();
        }
    }
}
