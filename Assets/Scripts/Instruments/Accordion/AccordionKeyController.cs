
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class AccordionKeyController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Accordion Key")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;

        [Header("UI References")]
        [SerializeField] private Image keyImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color pressedColor = new Color(1f, 0.9f, 0.4f);

        private AccordionController accordionController;
        private AudioSource activeVoice;
        private bool isPressed = false;

        public void Setup(string note, AudioClip clip, AccordionController accordion)
        {
            noteName = note;
            noteClip = clip;
            accordionController = accordion;

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
                activeVoice = InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            if (keyImage != null)
                keyImage.color = pressedColor;

            if (accordionController != null)
                accordionController.TriggerBellowsPump();

            StartCoroutine(InstrumentGameplayPanel.PunchScale(
                transform,
                new Vector3(1.15f, 0.9f, 1f),
                0.12f
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

            if (activeVoice != null && InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.StopSource(activeVoice, 0.1f);
                activeVoice = null;
            }

            if (keyImage != null)
                keyImage.color = normalColor;
        }

        private void OnDisable()
        {
            if (isPressed)
                Release();
        }
    }
}
