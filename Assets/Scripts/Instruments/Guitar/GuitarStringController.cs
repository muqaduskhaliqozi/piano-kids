
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PianoKids.Instruments
{
    public class GuitarStringController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        [Header("String Setup")]
        [SerializeField] private int stringIndex = 0;
        [SerializeField] private string stringName = "String1_E2";
        [SerializeField] private AudioClip pluckClip;

        [Header("Visual References")]
        [SerializeField] private RectTransform stringGraphic;
        [SerializeField] private Image stringLine;
        [SerializeField] private Color normalColor = new Color(0.9f, 0.85f, 0.75f, 0.9f);
        [SerializeField] private Color pluckColor = new Color(1f, 0.95f, 0.4f, 1f);

        private Coroutine vibrateRoutine;
        private Vector2 originalPos;

        private void Awake()
        {
            if (stringGraphic != null)
                originalPos = stringGraphic.anchoredPosition;
        }

        public void Setup(int index, string name, AudioClip clip)
        {
            stringIndex = index;
            stringName = name;
            pluckClip = clip;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pluck();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Strumming support when dragging finger/mouse across guitar strings
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

            vibrateRoutine = StartCoroutine(VibrationAnimation());
        }

        private IEnumerator VibrationAnimation()
        {
            if (stringGraphic == null) yield break;

            if (stringLine != null)
                stringLine.color = pluckColor;

            float duration = 0.45f;
            float elapsed = 0f;
            float amplitude = 8f;
            float freq = 45f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float decay = 1f - (elapsed / duration);
                float offset = Mathf.Sin(elapsed * freq * Mathf.PI * 2f) * amplitude * decay;

                stringGraphic.anchoredPosition = originalPos + new Vector2(0, offset);

                yield return null;
            }

            stringGraphic.anchoredPosition = originalPos;

            if (stringLine != null)
                stringLine.color = normalColor;

            vibrateRoutine = null;
        }
    }
}
