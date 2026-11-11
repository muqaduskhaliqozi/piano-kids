using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class PanFluteController : InstrumentGameplayPanel
    {
        [Header("Pan Flute Pipes")]
        [SerializeField] private List<PanFlutePipeController> pipes = new List<PanFlutePipeController>();

        private void Reset()
        {
            instrumentType = InstrumentType.PanFlute;
            instrumentDisplayName = "Pan Flute";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.PanFlute;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Pan Flute";

            base.Initialize(nav);

            pipes.RemoveAll(p => p == null);
            if (pipes.Count == 0)
            {
                pipes.AddRange(GetComponentsInChildren<PanFlutePipeController>(true));
            }
        }
    }
}
