
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PianoKids.Instruments
{
    public class HarpStringController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        [Header("Harp String Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip pluckClip;

        [Header("Visual References")]
        [SerializeField] private RectTransform stringGraphic;
        [SerializeField] private Image stringLine;
        [SerializeField] private Color normalColor = new Color(0.4f, 0.85f, 1f, 0.85f);
        [SerializeField] private Color pluckColor = new Color(1f, 0.95f, 0.5f, 1f);

        private Coroutine vibrateRoutine;
        private Vector2 originalPos;

        private void Awake()
        {
            if (stringGraphic != null)
                originalPos = stringGraphic.anchoredPosition;
        }

        public void Setup(string note, AudioClip clip, Color stringTint)
        {
            noteName = note;
            pluckClip = clip;
            normalColor = stringTint;

            if (stringLine != null)
                stringLine.color = normalColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pluck();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Glissando / slide across harp strings
            if (eventData.dragging || Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                Pluck();
            }
        }

        public void Pluck()
        {
            if (InstrumentAudioManager.Instance != null && pluckClip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(pluckClip, 1f);
            }

            // Floating music note effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );

            if (vibrateRoutine != null)
                StopCoroutine(vibrateRoutine);

            vibrateRoutine = StartCoroutine(VibrateString());
        }

        private IEnumerator VibrateString()
        {
            if (stringGraphic == null) yield break;

            if (stringLine != null)
                stringLine.color = pluckColor;

            float duration = 0.5f;
            float elapsed = 0f;
            float amplitude = 10f;
            float freq = 40f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float decay = 1f - (elapsed / duration);
                float offset = Mathf.Sin(elapsed * freq * Mathf.PI * 2f) * amplitude * decay;

                stringGraphic.anchoredPosition = originalPos + new Vector2(offset, 0);

                yield return null;
            }

            stringGraphic.anchoredPosition = originalPos;

            if (stringLine != null)
                stringLine.color = normalColor;

            vibrateRoutine = null;
        }
    }
}
