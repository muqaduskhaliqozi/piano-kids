
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectionController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string instrumentsSceneName = "Instruments";

    public void OnInstrumentsButtonClicked()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(instrumentsSceneName);
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play Mode will be developed later!");
    }
}
