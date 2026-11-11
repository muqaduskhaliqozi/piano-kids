using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class AccordionController : InstrumentGameplayPanel
    {
        [Header("Accordion Components")]
        [SerializeField] private RectTransform bellowsTransform;
        [SerializeField] private List<AccordionKeyController> keys = new List<AccordionKeyController>();

        private Coroutine bellowsRoutine;
        private Vector3 originalBellowsScale = Vector3.one;

        private void Reset()
        {
            instrumentType = InstrumentType.Accordion;
            instrumentDisplayName = "Accordion";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Accordion;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Accordion";

            base.Initialize(nav);

            if (bellowsTransform != null)
                originalBellowsScale = bellowsTransform.localScale;

            keys.RemoveAll(k => k == null);
            if (keys.Count == 0)
            {
                keys.AddRange(GetComponentsInChildren<AccordionKeyController>(true));
            }
        }

        public void TriggerBellowsPump()
        {
            if (bellowsTransform == null) return;
            if (bellowsRoutine != null) StopCoroutine(bellowsRoutine);
            bellowsRoutine = StartCoroutine(BellowsPumpAnimation());
        }

        private IEnumerator BellowsPumpAnimation()
        {
            float duration = 0.22f;
            float elapsed = 0f;
            Vector3 expandedScale = new Vector3(originalBellowsScale.x * 1.35f, originalBellowsScale.y, originalBellowsScale.z);

            while (elapsed < duration * 0.45f)
            {
                elapsed += Time.unscaledDeltaTime;
                bellowsTransform.localScale = Vector3.Lerp(originalBellowsScale, expandedScale, elapsed / (duration * 0.45f));
                yield return null;
            }

            elapsed = 0f;
            float remain = duration * 0.55f;
            while (elapsed < remain)
            {
                elapsed += Time.unscaledDeltaTime;
                bellowsTransform.localScale = Vector3.Lerp(expandedScale, originalBellowsScale, elapsed / remain);
                yield return null;
            }

            bellowsTransform.localScale = originalBellowsScale;
        }
    }
}
