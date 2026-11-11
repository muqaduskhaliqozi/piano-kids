
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class CatButtonController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Cat Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip meowClip;
        [SerializeField] private int catIndex = 1;

        [Header("Visual References")]
        [SerializeField] private Image catImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private GameObject singingMouth;
        [SerializeField] private GameObject noteBubble;

        private Coroutine singRoutine;
        private Vector3 originalScale = Vector3.one;
        private Vector3 originalPos;

        private void Awake()
        {
            originalScale = transform.localScale;
            originalPos = transform.localPosition;
        }

        public void Setup(string note, AudioClip clip, int index, Sprite catSprite)
        {
            noteName = note;
            meowClip = clip;
            catIndex = index;

            if (catImage != null && catSprite != null)
                catImage.sprite = catSprite;

            if (noteLabel != null)
                noteLabel.text = note.Substring(0, 1);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Meow();
        }

        public void Meow()
        {
            if (InstrumentAudioManager.Instance != null && meowClip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(meowClip, 1f);
            }

            // Floating music note effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );

            if (singRoutine != null)
                StopCoroutine(singRoutine);

            singRoutine = StartCoroutine(SingAnimation());
        }

        private IEnumerator SingAnimation()
        {
            if (singingMouth != null)
                singingMouth.SetActive(true);

            if (noteBubble != null)
                noteBubble.SetActive(true);

            // Joyful bounce & stretch
            Vector3 bouncePos = originalPos + new Vector3(0, 22f, 0);

            Vector3 stretchScale = new Vector3(
                originalScale.x * 0.92f,
                originalScale.y * 1.25f,
                originalScale.z
            );

            Vector3 squashScale = new Vector3(
                originalScale.x * 1.15f,
                originalScale.y * 0.9f,
                originalScale.z
            );

            float duration = 0.28f;
            float elapsed = 0f;

            // Up & stretch
            while (elapsed < duration * 0.4f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (duration * 0.4f);

                transform.localPosition = Vector3.Lerp(
                    originalPos,
                    bouncePos,
                    t
                );

                transform.localScale = Vector3.Lerp(
                    originalScale,
                    stretchScale,
                    t
                );

                yield return null;
            }

            // Down & squash
            elapsed = 0f;

            while (elapsed < duration * 0.4f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (duration * 0.4f);

                transform.localPosition = Vector3.Lerp(
                    bouncePos,
                    originalPos,
                    t
                );

                transform.localScale = Vector3.Lerp(
                    stretchScale,
                    squashScale,
                    t
                );

                yield return null;
            }

            // Rebound to normal
            elapsed = 0f;

            while (elapsed < duration * 0.2f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / (duration * 0.2f);

                transform.localScale = Vector3.Lerp(
                    squashScale,
                    originalScale,
                    t
                );

                yield return null;
            }

            transform.localPosition = originalPos;
            transform.localScale = originalScale;

            if (singingMouth != null)
                singingMouth.SetActive(false);

            if (noteBubble != null)
                noteBubble.SetActive(false);

            singRoutine = null;
        }
    }
}
