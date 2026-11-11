using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class HarmonicaController : InstrumentGameplayPanel
    {
        [Header("Harmonica Holes")]
        [SerializeField] private List<HarmonicaHoleController> holes = new List<HarmonicaHoleController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Harmonica;
            instrumentDisplayName = "Harmonica";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Harmonica;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Harmonica";

            base.Initialize(nav);

            holes.RemoveAll(h => h == null);
            if (holes.Count == 0)
            {
                holes.AddRange(GetComponentsInChildren<HarmonicaHoleController>(true));
            }
        }
    }
}
