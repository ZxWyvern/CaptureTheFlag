using UnityEngine;
using TMPro;
public class UiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameManager gameManager;
    public GameObject gameOverPanel;
    public TextMeshProUGUI healthText;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Update()
    {
        if (gameManager.gamePhase == GamePhase.Retry)
        {
            ShowGameOverPanel();
        }
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString();
        }
    }
}
