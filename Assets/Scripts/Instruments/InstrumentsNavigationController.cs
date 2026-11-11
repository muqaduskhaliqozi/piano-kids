using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PianoKids.Instruments
{
    public class InstrumentsNavigationController : MonoBehaviour
    {
        [Header("Main Panels")]
        [SerializeField] private GameObject instrumentsSelectionPanel;
        [SerializeField] private GameObject instrumentGameplayPanel;

        [Header("Controllers")]
        [SerializeField] private InstrumentSelectionController selectionScreen;
        [SerializeField] private List<InstrumentGameplayPanel> gameplayPanels = new List<InstrumentGameplayPanel>();

        [Header("Gameplay Header UI")]
        [SerializeField] private Button gameplayBackButton;
        [SerializeField] private TMP_Text gameplayTitleText;
        [SerializeField] private Button gameplaySoundButton;
        [SerializeField] private Image gameplaySoundIcon;

        private readonly Dictionary<InstrumentType, InstrumentGameplayPanel> panelMap =
            new Dictionary<InstrumentType, InstrumentGameplayPanel>();

        private InstrumentGameplayPanel currentActivePanel = null;

        public InstrumentGameplayPanel CurrentActivePanel => currentActivePanel;

        private void Awake()
        {
            AutoResolveReferences();
            BuildPanelMap();
        }

        private void Start()
        {
            SetupNavigation();
        }

        public void AutoResolveReferences()
        {
            // 1. Resolve Selection Panel & Controller
            if (instrumentsSelectionPanel == null)
            {
                var sel = transform.Find("InstrumentsSelectionPanel");
                if (sel != null) instrumentsSelectionPanel = sel.gameObject;
            }

            if (selectionScreen == null && instrumentsSelectionPanel != null)
            {
                selectionScreen = instrumentsSelectionPanel.GetComponent<InstrumentSelectionController>();
            }

            // 2. Resolve Gameplay Parent Panel
            if (instrumentGameplayPanel == null)
            {
                var gp = transform.Find("InstrumentGameplayPanel");
                if (gp != null) instrumentGameplayPanel = gp.gameObject;
            }

            // 3. Resolve Gameplay Header UI
            if (instrumentGameplayPanel != null)
            {
                if (gameplayBackButton == null)
                {
                    var back = instrumentGameplayPanel.transform.Find("GameplayHeader/BackButton");
                    if (back != null) gameplayBackButton = back.GetComponent<Button>();
                }

                if (gameplayTitleText == null)
                {
                    var title = instrumentGameplayPanel.transform.Find("GameplayHeader/InstrumentTitle");
                    if (title != null) gameplayTitleText = title.GetComponent<TMP_Text>();
                }

                if (gameplaySoundButton == null)
                {
                    var sound = instrumentGameplayPanel.transform.Find("GameplayHeader/SoundSettingsButton");
                    if (sound != null)
                    {
                        gameplaySoundButton = sound.GetComponent<Button>();
                        gameplaySoundIcon = sound.GetComponent<Image>();
                    }
                }

                // 4. Resolve Gameplay Panels if list is empty or contains nulls
                gameplayPanels.RemoveAll(p => p == null);
                if (gameplayPanels.Count == 0)
                {
                    var foundPanels = instrumentGameplayPanel.GetComponentsInChildren<InstrumentGameplayPanel>(true);
                    gameplayPanels.AddRange(foundPanels);
                }
            }
        }

        private void BuildPanelMap()
        {
            panelMap.Clear();
            foreach (var panel in gameplayPanels)
            {
                if (panel != null)
                {
                    panelMap[panel.Type] = panel;
                }
            }
        }

        private void SetupNavigation()
        {
            // Initial state: SelectionPanel = true, GameplayPanel = false, all child panels = false
            if (instrumentsSelectionPanel != null)
            {
                instrumentsSelectionPanel.SetActive(true);
            }

            if (instrumentGameplayPanel != null)
            {
                instrumentGameplayPanel.SetActive(false);
            }

            foreach (var panel in gameplayPanels)
            {
                if (panel != null)
                {
                    panel.Initialize(this);
                    panel.Close();
                }
            }

            if (selectionScreen != null)
            {
                selectionScreen.Initialize(this);
                selectionScreen.Show();
            }

            // Connect Gameplay Header BackButton
            if (gameplayBackButton != null)
            {
                gameplayBackButton.onClick.RemoveAllListeners();
                gameplayBackButton.onClick.AddListener(ReturnToSelection);
            }

            // Connect Gameplay Header SoundButton
            if (gameplaySoundButton != null)
            {
                gameplaySoundButton.onClick.RemoveAllListeners();
                gameplaySoundButton.onClick.AddListener(ToggleSound);
                UpdateSoundIcon();
            }
        }

        public void OpenInstrument(InstrumentType type)
        {
            // Avoid duplicate open if already on this instrument
            if (currentActivePanel != null && currentActivePanel.Type == type &&
                instrumentGameplayPanel != null && instrumentGameplayPanel.activeSelf)
            {
                return;
            }

            // 1. Hide Selection Screen
            if (selectionScreen != null)
            {
                selectionScreen.Hide();
            }
            if (instrumentsSelectionPanel != null)
            {
                instrumentsSelectionPanel.SetActive(false);
            }

            // 2. Activate Gameplay Parent Panel FIRST (so children become visible & active)
            if (instrumentGameplayPanel != null)
            {
                instrumentGameplayPanel.SetActive(true);
            }

            // 3. Deactivate all instrument child panels
            foreach (var p in gameplayPanels)
            {
                if (p != null)
                {
                    p.Close();
                }
            }

            // 4. Activate target instrument panel
            if (panelMap.TryGetValue(type, out var panel) && panel != null)
            {
                currentActivePanel = panel;
                panel.Open();

                if (gameplayTitleText != null)
                {
                    gameplayTitleText.text = panel.DisplayName.ToUpper();
                }
            }
            else
            {
                Debug.LogWarning($"[InstrumentsNavigation] No panel registered for instrument: {type}");
                ReturnToSelection();
            }

            UpdateSoundIcon();
        }

        public void ReturnToSelection()
        {
            // Close active gameplay panel
            if (currentActivePanel != null)
            {
                currentActivePanel.Close();
                currentActivePanel = null;
            }

            foreach (var p in gameplayPanels)
            {
                if (p != null)
                {
                    p.Close();
                }
            }

            // Hide Gameplay Parent Panel
            if (instrumentGameplayPanel != null)
            {
                instrumentGameplayPanel.SetActive(false);
            }

            // Show Selection Panel
            if (instrumentsSelectionPanel != null)
            {
                instrumentsSelectionPanel.SetActive(true);
            }

            if (selectionScreen != null)
            {
                selectionScreen.Show();
            }
        }

        public void ToggleSound()
        {
            if (InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.ToggleMute();
                UpdateSoundIcon();
            }
        }

        private void UpdateSoundIcon()
        {
            if (gameplaySoundIcon != null && InstrumentAudioManager.Instance != null)
            {
                gameplaySoundIcon.color = InstrumentAudioManager.Instance.IsMuted
                    ? new Color(0.7f, 0.7f, 0.7f, 0.6f)
                    : Color.white;
            }
        }

        // Direct methods for Button OnClick bindings in Inspector
        public void OpenPiano() => OpenInstrument(InstrumentType.Piano);
        public void OpenXylophone() => OpenInstrument(InstrumentType.Xylophone);
        public void OpenDrums() => OpenInstrument(InstrumentType.Drums);
        public void OpenFlute() => OpenInstrument(InstrumentType.Flute);
        public void OpenGuitar() => OpenInstrument(InstrumentType.Guitar);
        public void OpenAccordion() => OpenInstrument(InstrumentType.Accordion);
        public void OpenSaxophone() => OpenInstrument(InstrumentType.Saxophone);
        public void OpenHarp() => OpenInstrument(InstrumentType.Harp);
        public void OpenPanFlute() => OpenInstrument(InstrumentType.PanFlute);
        public void OpenCats() => OpenInstrument(InstrumentType.Cats);
        public void OpenHarmonica() => OpenInstrument(InstrumentType.Harmonica);
    }
}
