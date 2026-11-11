
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class SaxophoneKeyController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Saxophone Key Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;

        [Header("UI References")]
        [SerializeField] private Image pearlKeyImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private Color normalColor = new Color(0.95f, 0.95f, 0.98f);
        [SerializeField] private Color pressedColor = new Color(1f, 0.8f, 0.2f);

        private SaxophoneController saxController;
        private AudioSource activeSource;
        private bool isPressed = false;

        public void Setup(string note, AudioClip clip, SaxophoneController sax)
        {
            noteName = note;
            noteClip = clip;
            saxController = sax;

            if (noteLabel != null)
                noteLabel.text = note.Substring(0, 1);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Press();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Release();
        }

        private void Press()
        {
            if (isPressed) return;
            isPressed = true;

            if (InstrumentAudioManager.Instance != null && noteClip != null)
            {
                activeSource = InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            if (pearlKeyImage != null)
                pearlKeyImage.color = pressedColor;

            if (saxController != null)
                saxController.TriggerBellWave();

            StartCoroutine(InstrumentGameplayPanel.PunchScale(
                transform,
                new Vector3(1.22f, 1.22f, 1f),
                0.14f
            ));

            // Floating music note effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );
        }

        private void Release()
        {
            if (!isPressed) return;
            isPressed = false;

            if (activeSource != null && InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.StopSource(activeSource, 0.1f);
                activeSource = null;
            }

            if (pearlKeyImage != null)
                pearlKeyImage.color = normalColor;
        }

        private void OnDisable()
        {
            if (isPressed) Release();
        }
    }
}
