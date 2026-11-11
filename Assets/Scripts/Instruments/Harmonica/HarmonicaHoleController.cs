
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class HarmonicaHoleController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Harmonica Hole Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;
        [SerializeField] private int holeNumber = 1;

        [Header("Visual References")]
        [SerializeField] private Image holeImage;
        [SerializeField] private Image airFlowGlow;
        [SerializeField] private TMP_Text holeNumberText;
        [SerializeField] private Color normalColor = new Color(0.2f, 0.25f, 0.35f);
        [SerializeField] private Color pressedColor = new Color(0.95f, 0.75f, 0.2f);

        private AudioSource activeVoice;
        private bool isPressed = false;

        public void Setup(string note, AudioClip clip, int number)
        {
            noteName = note;
            noteClip = clip;
            holeNumber = number;

            if (holeNumberText != null)
                holeNumberText.text = number.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Blow();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StopBlowing();
        }

        private void Blow()
        {
            if (isPressed) return;

            isPressed = true;

            if (InstrumentAudioManager.Instance != null && noteClip != null)
            {
                activeVoice = InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            if (holeImage != null)
                holeImage.color = pressedColor;

            if (airFlowGlow != null)
                airFlowGlow.gameObject.SetActive(true);

            StartCoroutine(InstrumentGameplayPanel.PunchScale(
                transform,
                new Vector3(1.15f, 1.25f, 1f),
                0.14f
            ));

            // Floating music note effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );
        }

        private void StopBlowing()
        {
            if (!isPressed) return;

            isPressed = false;

            if (activeVoice != null && InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.StopSource(activeVoice, 0.08f);
                activeVoice = null;
            }

            if (holeImage != null)
                holeImage.color = normalColor;

            if (airFlowGlow != null)
                airFlowGlow.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (isPressed)
                StopBlowing();
        }
    }
}
