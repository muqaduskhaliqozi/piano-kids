
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class XylophoneBarController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Bar Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;
        [SerializeField] private Color barColor = Color.red;

        [Header("UI References")]
        [SerializeField] private Image barImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private GameObject hitSparkle;

        private Coroutine bounceRoutine;

        public string NoteName => noteName;

        public void Setup(string note, AudioClip clip, Color color)
        {
            noteName = note;
            noteClip = clip;
            barColor = color;

            if (barImage != null)
                barImage.color = color;

            if (noteLabel != null)
                noteLabel.text = note.Substring(0, 1);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HitBar();
        }

        public void HitBar()
        {
            // Play xylophone sound
            if (InstrumentAudioManager.Instance != null &&
                noteClip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            // Bounce animation
            if (bounceRoutine != null)
                StopCoroutine(bounceRoutine);

            bounceRoutine = StartCoroutine(BounceAnimation());

            // Existing sparkle effect
            if (hitSparkle != null)
            {
                hitSparkle.SetActive(false);
                hitSparkle.SetActive(true);
            }

            // Floating Music Note Effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );
        }

        private IEnumerator BounceAnimation()
        {
            Vector3 originalScale = Vector3.one;
            Vector3 punchScale = new Vector3(1.1f, 0.88f, 1f);

            float duration = 0.16f;
            float elapsed = 0f;

            while (elapsed < duration * 0.4f)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localScale = Vector3.Lerp(
                    originalScale,
                    punchScale,
                    elapsed / (duration * 0.4f)
                );

                yield return null;
            }

            elapsed = 0f;
            float remain = duration * 0.6f;

            while (elapsed < remain)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localScale = Vector3.Lerp(
                    punchScale,
                    originalScale,
                    elapsed / remain
                );

                yield return null;
            }

            transform.localScale = originalScale;
            bounceRoutine = null;
        }
    }
}
