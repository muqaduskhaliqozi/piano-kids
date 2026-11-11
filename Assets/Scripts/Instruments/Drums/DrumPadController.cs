
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PianoKids.Instruments
{
    public class DrumPadController : MonoBehaviour, IPointerDownHandler
    {
        [Header("Drum Pad Setup")]
        [SerializeField] private string drumName = "BassDrum";
        [SerializeField] private AudioClip drumClip;

        [Header("Visual References")]
        [SerializeField] private Image padImage;
        [SerializeField] private GameObject hitEffect;

        private Coroutine hitRoutine;

        public void Setup(string name, AudioClip clip)
        {
            drumName = name;
            drumClip = clip;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HitDrum();
        }

        public void HitDrum()
        {
            // Play drum sound
            if (InstrumentAudioManager.Instance != null &&
                drumClip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(drumClip, 1f);
            }

            // Drum hit animation
            if (hitRoutine != null)
                StopCoroutine(hitRoutine);

            hitRoutine = StartCoroutine(DrumHitAnimation());

            // Existing hit effect
            if (hitEffect != null)
            {
                hitEffect.SetActive(false);
                hitEffect.SetActive(true);
            }

            // Floating Music Note Effect
            FloatingMusicNoteEffectSpawner.SpawnFor(
                GetComponent<RectTransform>()
            );
        }

        private IEnumerator DrumHitAnimation()
        {
            Vector3 originalScale = Vector3.one;
            Vector3 punch = new Vector3(1.18f, 1.18f, 1f);

            float duration = 0.18f;
            float elapsed = 0f;

            while (elapsed < duration * 0.35f)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localScale = Vector3.Lerp(
                    originalScale,
                    punch,
                    elapsed / (duration * 0.35f)
                );

                yield return null;
            }

            elapsed = 0f;
            float remain = duration * 0.65f;

            while (elapsed < remain)
            {
                elapsed += Time.unscaledDeltaTime;

                transform.localScale = Vector3.Lerp(
                    punch,
                    originalScale,
                    elapsed / remain
                );

                yield return null;
            }

            transform.localScale = originalScale;
            hitRoutine = null;
        }
    }
}
