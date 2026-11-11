using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class FluteController : InstrumentGameplayPanel
    {
        [Header("Flute Holes")]
        [SerializeField] private List<FluteHoleController> holes = new List<FluteHoleController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Flute;
            instrumentDisplayName = "Flute";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Flute;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Flute";

            base.Initialize(nav);

            holes.RemoveAll(h => h == null);
            if (holes.Count == 0)
            {
                holes.AddRange(GetComponentsInChildren<FluteHoleController>(true));
            }
        }
    }
}
