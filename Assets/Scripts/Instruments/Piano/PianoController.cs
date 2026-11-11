using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class PianoController : InstrumentGameplayPanel
    {
        [Header("Piano Keys Setup")]
        [SerializeField] private List<PianoKeyController> pianoKeys = new List<PianoKeyController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Piano;
            instrumentDisplayName = "Piano";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Piano;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Piano";

            base.Initialize(nav);

            pianoKeys.RemoveAll(k => k == null);
            if (pianoKeys.Count == 0)
            {
                pianoKeys.AddRange(GetComponentsInChildren<PianoKeyController>(true));
            }
        }
    }
}
