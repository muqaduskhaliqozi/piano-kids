using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PianoKids.Instruments
{
    public class InstrumentSelectionController : MonoBehaviour
    {
        [Header("Scene Navigation")]
        [SerializeField] private string modeSelectionSceneName = "ModeSelection";
        [SerializeField] private Button backButton;

        [Header("Header Elements")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button soundToggleButton;
        [SerializeField] private Image soundToggleIcon;

        [Header("Cards Container")]
        [SerializeField] private List<InstrumentCardController> instrumentCards = new List<InstrumentCardController>();

        [Header("Navigation Controller")]
        [SerializeField] private InstrumentsNavigationController navigationController;

        private void Awake()
        {
            AutoResolveReferences();
        }

        public void AutoResolveReferences()
        {
            if (navigationController == null)
            {
                navigationController = FindObjectOfType<InstrumentsNavigationController>();
            }

            if (backButton == null)
            {
                var b = transform.Find("Header/BackButton");
                if (b != null) backButton = b.GetComponent<Button>();
            }

            if (titleText == null)
            {
                var t = transform.Find("Header/TitleText");
                if (t != null) titleText = t.GetComponent<TMP_Text>();
            }

            if (soundToggleButton == null)
            {
                var s = transform.Find("Header/SoundSettingsButton");
                if (s != null)
                {
                    soundToggleButton = s.GetComponent<Button>();
                    soundToggleIcon = s.GetComponent<Image>();
                }
            }

            instrumentCards.RemoveAll(c => c == null);
            if (instrumentCards.Count == 0)
            {
                var foundCards = GetComponentsInChildren<InstrumentCardController>(true);
                instrumentCards.AddRange(foundCards);
            }
        }

        public void Initialize(InstrumentsNavigationController nav)
        {
            navigationController = nav;
            AutoResolveReferences();

            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(OnBackButtonClicked);
            }

            if (soundToggleButton != null)
            {
                soundToggleButton.onClick.RemoveAllListeners();
                soundToggleButton.onClick.AddListener(OnSoundToggleClicked);
                UpdateSoundIcon();
            }

            // Wire all cards
            foreach (var card in instrumentCards)
            {
                if (card != null)
                {
                    card.OnCardClicked -= OnCardSelected;
                    card.OnCardClicked += OnCardSelected;
                }
            }
        }

        public void OnBackButtonClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(modeSelectionSceneName);
        }

        public void OnSoundToggleClicked()
        {
            if (InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.ToggleMute();
                UpdateSoundIcon();
            }
        }

        public void UpdateSoundIcon()
        {
            if (soundToggleIcon != null && InstrumentAudioManager.Instance != null)
            {
                soundToggleIcon.color = InstrumentAudioManager.Instance.IsMuted
                    ? new Color(0.7f, 0.7f, 0.7f, 0.6f)
                    : Color.white;
            }
        }

        private void OnCardSelected(InstrumentType type)
        {
            if (navigationController == null)
                navigationController = FindObjectOfType<InstrumentsNavigationController>();

            if (navigationController != null)
            {
                navigationController.OpenInstrument(type);
            }
            else
            {
                Debug.LogError("[InstrumentSelectionController] InstrumentsNavigationController not found!");
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateSoundIcon();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void RegisterCard(InstrumentCardController card)
        {
            if (card != null && !instrumentCards.Contains(card))
            {
                instrumentCards.Add(card);
                card.OnCardClicked -= OnCardSelected;
                card.OnCardClicked += OnCardSelected;
            }
        }
    }
}
