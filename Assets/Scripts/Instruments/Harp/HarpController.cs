using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class HarpController : InstrumentGameplayPanel
    {
        [Header("Harp Strings")]
        [SerializeField] private List<HarpStringController> strings = new List<HarpStringController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Harp;
            instrumentDisplayName = "Harp";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Harp;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Harp";

            base.Initialize(nav);

            strings.RemoveAll(s => s == null);
            if (strings.Count == 0)
            {
                strings.AddRange(GetComponentsInChildren<HarpStringController>(true));
            }
        }
    }
}
