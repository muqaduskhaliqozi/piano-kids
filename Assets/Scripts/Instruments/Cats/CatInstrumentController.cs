using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class CatInstrumentController : InstrumentGameplayPanel
    {
        [Header("Cat Characters")]
        [SerializeField] private List<CatButtonController> catButtons = new List<CatButtonController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Cats;
            instrumentDisplayName = "Cats";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Cats;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Cats";

            base.Initialize(nav);

            catButtons.RemoveAll(c => c == null);
            if (catButtons.Count == 0)
            {
                catButtons.AddRange(GetComponentsInChildren<CatButtonController>(true));
            }
        }
    }
}
