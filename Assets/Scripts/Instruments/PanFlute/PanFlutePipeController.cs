
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class PanFlutePipeController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Pipe Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;

        [Header("Visual References")]
        [SerializeField] private Image pipeImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private Color normalColor = new Color(0.88f, 0.72f, 0.48f);
        [SerializeField] private Color pressedColor = new Color(1f, 0.9f, 0.45f);

        private Coroutine bounceRoutine;

        public void Setup(string note, AudioClip clip)
        {
            noteName = note;
            noteClip = clip;

            if (noteLabel != null)
                noteLabel.text = note.Substring(0, 1);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayPipe();
        }

        public void PlayPipe()
        {
            if (InstrumentAudioManager.Instance != null && noteClip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            // Floating music note effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );

            if (bounceRoutine != null)
                StopCoroutine(bounceRoutine);

            bounceRoutine = StartCoroutine(PipeBounce());
        }

        private IEnumerator PipeBounce()
        {
            if (pipeImage != null)
                pipeImage.color = pressedColor;

            Vector3 startPos = transform.localPosition;
            Vector3 downPos = startPos + new Vector3(0, -12f, 0);

            float duration = 0.16f;
            float elapsed = 0f;

            while (elapsed < duration * 0.4f)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localPosition = Vector3.Lerp(
                    startPos,
                    downPos,
                    elapsed / (duration * 0.4f)
                );

                yield return null;
            }

            elapsed = 0f;
            float remain = duration * 0.6f;

            while (elapsed < remain)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localPosition = Vector3.Lerp(
                    downPos,
                    startPos,
                    elapsed / remain
                );

                yield return null;
            }

            transform.localPosition = startPos;

            if (pipeImage != null)
                pipeImage.color = normalColor;

            bounceRoutine = null;
        }
    }
}
