using UnityEngine;
using UnityEngine.SceneManagement;
public enum GamePhase
{
    Retry, GamePlay, Menu , Credit
}
public class GameManager : MonoBehaviour
{
    public GamePhase gamePhase;
    private AudioSource audioSource;
    

    void Start()
    {
        gamePhase = GamePhase.GamePlay;
        audioSource = GetComponent<AudioSource>();
        
    }
    public void Ui()
    {
        gamePhase = GamePhase.Credit;
    }
    public void Over()
    {
        gamePhase = GamePhase.Retry;

    }
        public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
