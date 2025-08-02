using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class OnClickSound : MonoBehaviour
{
    public AudioClip buttonAudioClip; // Referensi ke AudioClip
    private AudioSource audioSource;
    private GameObject whiteScreenOverlay;
    private Image whiteScreenImage;

    private void Awake()
    {
        // Cek apakah sudah ada AudioSource, jika belum tambahkan
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Buat white screen overlay secara dinamis
        CreateWhiteScreenOverlay();
    }

    private void CreateWhiteScreenOverlay()
    {
        // Buat Canvas baru
        GameObject canvasGO = new GameObject("WhiteScreenCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // Pastikan overlay di atas UI lain

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Buat Image putih sebagai overlay
        whiteScreenOverlay = new GameObject("WhiteScreen");
        whiteScreenOverlay.transform.SetParent(canvasGO.transform, false);
        whiteScreenImage = whiteScreenOverlay.AddComponent<Image>();
        whiteScreenImage.color = new Color(1f, 1f, 1f, 0f); // Mulai dengan alpha 0

        // Set ukuran overlay memenuhi layar
        RectTransform rectTransform = whiteScreenOverlay.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Sembunyikan overlay pada awalnya
        whiteScreenOverlay.SetActive(false);
    }

    // Fungsi ini akan dipanggil saat tombol diklik
    public void PlaySound()
    {
        if (buttonAudioClip != null)
        {
            StartCoroutine(PlaySoundAndReloadScene());
        }
        else
        {
            Debug.LogWarning("AudioClip tidak ditemukan pada OnClickSound!");
        }
    }

    private IEnumerator PlaySoundAndReloadScene()
    {
        audioSource.PlayOneShot(buttonAudioClip);

        // Tampilkan white screen overlay dan mulai fade in
        whiteScreenOverlay.SetActive(true);
        yield return StartCoroutine(FadeInWhiteScreen(1f));

        // Tunggu sisa waktu agar total delay 2 detik
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator FadeInWhiteScreen(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            whiteScreenImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        whiteScreenImage.color = Color.white; // Pastikan alpha 1 di akhir
    }

    // New coroutine to load scene with 2 seconds delay
    public IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    // Public method to be called on button click to start the coroutine
    public void OnButtonClickLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithDelay(sceneName));
    }
    public void OnButtonClickLoadScene2()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
