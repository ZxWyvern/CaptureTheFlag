using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OnTriggerSound : MonoBehaviour
{
    public AudioClip triggerAudioClip; // Referensi ke AudioClip
    public string sceneToLoad; // Scene name to load on TMP text click
    public GameObject continueTextObject; // Assign in inspector, TMP text GameObject (ensure its Canvas sortingOrder is set to 1001)
    public GameObject additionalTextObject; // New additional text GameObject to activate after fade
    public GameObject thirdTextObject; // Another text GameObject to activate after fade
    private AudioSource audioSource;
    private GameObject blackScreenOverlay;
    private Image blackScreenImage;
    private bool hasTriggered = false; // To prevent multiple triggers

    private void Awake()
    {
        // Cek apakah sudah ada AudioSource, jika belum tambahkan
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Buat black screen overlay secara dinamis
        CreateBlackScreenOverlay();

        if (continueTextObject != null)
        {
            continueTextObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("ContinueTextObject GameObject is not assigned in inspector.");
        }

        if (additionalTextObject != null)
        {
            additionalTextObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("AdditionalTextObject GameObject is not assigned in inspector.");
        }

        if (thirdTextObject != null)
        {
            thirdTextObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("ThirdTextObject GameObject is not assigned in inspector.");
        }
    }

    private void CreateBlackScreenOverlay()
    {
        // Buat Canvas baru
        GameObject canvasGO = new GameObject("BlackScreenCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1001; // Changed layer to 1001 as requested

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Buat Image hitam sebagai overlay
        blackScreenOverlay = new GameObject("BlackScreen");
        blackScreenOverlay.transform.SetParent(canvasGO.transform, false);
        blackScreenImage = blackScreenOverlay.AddComponent<Image>();
        blackScreenImage.color = new Color(0f, 0f, 0f, 0f); // Mulai dengan alpha 0 (black)

        // Set ukuran overlay memenuhi layar
        RectTransform rectTransform = blackScreenOverlay.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Sembunyikan overlay pada awalnya
        blackScreenOverlay.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (triggerAudioClip != null)
            {
                StartCoroutine(PlaySoundAndFade());
            }
            else
            {
                Debug.LogWarning("AudioClip tidak ditemukan pada OnTriggerSound!");
            }
        }
    }

    private IEnumerator PlaySoundAndFade()
    {
        audioSource.PlayOneShot(triggerAudioClip);

        if (blackScreenOverlay != null && blackScreenImage != null)
        {
            blackScreenOverlay.SetActive(true);
            yield return StartCoroutine(FadeInBlackScreen(1f));
        }

        if (continueTextObject != null)
        {
            continueTextObject.SetActive(true);
        }

        if (additionalTextObject != null)
        {
            additionalTextObject.SetActive(true);
        }

        if (thirdTextObject != null)
        {
            thirdTextObject.SetActive(true);
        }
    }

    private IEnumerator FadeInBlackScreen(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            if (blackScreenImage != null)
            {
                blackScreenImage.color = new Color(0f, 0f, 0f, alpha);
            }
            yield return null;
        }
        if (blackScreenImage != null)
        {
            blackScreenImage.color = new Color(0f, 0f, 0f, 1f);
        }
    }

    // This method should be linked to the Button component on continueTextObject in the inspector
    public void OnContinueTextClicked()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not set in OnTriggerSound.");
        }
    }
}
