using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class SaxophoneController : InstrumentGameplayPanel
    {
        [Header("Saxophone Components")]
        [SerializeField] private RectTransform bellWaveTransform;
        [SerializeField] private List<SaxophoneKeyController> keys = new List<SaxophoneKeyController>();

        private Coroutine bellRoutine;

        private void Reset()
        {
            instrumentType = InstrumentType.Saxophone;
            instrumentDisplayName = "Saxophone";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Saxophone;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Saxophone";

            base.Initialize(nav);

            keys.RemoveAll(k => k == null);
            if (keys.Count == 0)
            {
                keys.AddRange(GetComponentsInChildren<SaxophoneKeyController>(true));
            }
        }

        public void TriggerBellWave()
        {
            if (bellWaveTransform == null) return;
            if (bellRoutine != null) StopCoroutine(bellRoutine);
            bellRoutine = StartCoroutine(BellWaveAnimation());
        }

        private IEnumerator BellWaveAnimation()
        {
            bellWaveTransform.gameObject.SetActive(true);
            float duration = 0.35f;
            float elapsed = 0f;
            Vector3 startScale = new Vector3(0.5f, 0.5f, 1f);
            Vector3 targetScale = new Vector3(1.6f, 1.6f, 1f);

            CanvasGroup cg = bellWaveTransform.GetComponent<CanvasGroup>();

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = elapsed / duration;
                bellWaveTransform.localScale = Vector3.Lerp(startScale, targetScale, progress);
                if (cg != null) cg.alpha = 1f - progress;
                yield return null;
            }

            bellWaveTransform.gameObject.SetActive(false);
        }
    }
}
