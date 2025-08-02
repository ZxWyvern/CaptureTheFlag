using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip damageSound;
    public TextMeshProUGUI Health;
    private UiController uiController;
    private bool hasPlayedHitSound = false;
    public AudioClip hitSound;
    

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        uiController = FindAnyObjectByType<UiController>();
        if (uiController != null)
        {
            uiController.UpdateHealthDisplay(currentHealth);
        }
    }
    private void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (audioSource != null && damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }
            if (uiController != null)
            {
                uiController.UpdateHealthDisplay(currentHealth);
            }
            StartCoroutine(InvincibilityCoroutine());
            StartCoroutine(DamageFlashCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private IEnumerator DamageFlashCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Die()
    {
        // Disable all animations when health reaches 0
        if (animator != null)
        {
            animator.enabled = false;
        }
        // Reload the current scene on death
        gameManager.Over();
        if (!hasPlayedHitSound && audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
            hasPlayedHitSound = true;
        }   
    }
}
