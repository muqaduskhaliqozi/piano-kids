
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class FluteHoleController : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [Header("Flute Hole Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;

        [Header("UI References")]
        [SerializeField] private Image holeImage;
        [SerializeField] private Image glowRing;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField]
        private Color normalColor =
            new Color(0.35f, 0.75f, 0.95f);
        [SerializeField]
        private Color pressedColor =
            new Color(1f, 0.85f, 0.2f);

        private AudioSource activeSource;
        private bool isPressed = false;

        public void Setup(string note, AudioClip clip)
        {
            noteName = note;
            noteClip = clip;

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

            // Play flute sound
            if (InstrumentAudioManager.Instance != null &&
                noteClip != null)
            {
                activeSource =
                    InstrumentAudioManager.Instance.PlaySound(
                        noteClip, 1f);
            }

            // Press visual
            if (holeImage != null)
                holeImage.color = pressedColor;

            if (glowRing != null)
                glowRing.gameObject.SetActive(true);

            StartCoroutine(
                InstrumentGameplayPanel.PunchScale(
                    transform,
                    new Vector3(1.2f, 1.2f, 1f),
                    0.15f
                )
            );

            // Floating Music Note Effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );
        }

        private void Release()
        {
            if (!isPressed) return;

            isPressed = false;

            if (activeSource != null &&
                InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.StopSource(
                    activeSource, 0.08f);

                activeSource = null;
            }

            if (holeImage != null)
                holeImage.color = normalColor;

            if (glowRing != null)
                glowRing.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (isPressed)
                Release();
        }
    }
}
