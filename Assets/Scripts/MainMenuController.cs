
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string modeSelectionSceneName = "ModeSelection";

    public void OnPlayButtonClicked()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(modeSelectionSceneName);
    }
}
