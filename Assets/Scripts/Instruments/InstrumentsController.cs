using UnityEngine;

namespace PianoKids.Instruments
{
    public class InstrumentsController : MonoBehaviour
    {
        [Header("Scene Controllers")]
        [SerializeField] private InstrumentsNavigationController navigationController;
        [SerializeField] private InstrumentAudioManager audioManager;

        private void Awake()
        {
            Time.timeScale = 1f;

            if (navigationController == null)
                navigationController = FindObjectOfType<InstrumentsNavigationController>();

            if (audioManager == null)
                audioManager = FindObjectOfType<InstrumentAudioManager>();
        }
    }
}
