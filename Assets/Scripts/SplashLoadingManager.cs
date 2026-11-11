
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SplashLoadingManager : MonoBehaviour
{
    [Header("Loading Settings")]
    [SerializeField] private float loadingDuration = 6f;
    [SerializeField] private string nextSceneName = "MainMenu";

    [Header("Loading UI")]
    [SerializeField] private Image loadingFill;
    [SerializeField] private TMP_Text loadingPercentage;
    [SerializeField] private GameObject loadingArea;

    private void Start()
    {
        Time.timeScale = 1f;

        if (loadingFill != null)
            loadingFill.fillAmount = 0f;

        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(nextSceneName);

        operation.allowSceneActivation = false;

        float timer = 0f;

        while (timer < loadingDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(timer / loadingDuration);

            if (loadingFill != null)
                loadingFill.fillAmount = progress;

            if (loadingPercentage != null)
                loadingPercentage.text =
                    Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        if (loadingFill != null)
            loadingFill.fillAmount = 1f;

        if (loadingPercentage != null)
            loadingPercentage.text = "100%";

        // Wait until Unity has finished loading the next scene.
        while (!operation.isDone && operation.progress < 0.9f)
        {
            yield return null;
        }

        if (loadingArea != null)
            loadingArea.SetActive(false);

        operation.allowSceneActivation = true;
    }
}
