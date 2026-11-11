using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class XylophoneController : InstrumentGameplayPanel
    {
        [Header("Xylophone Bars")]
        [SerializeField] private List<XylophoneBarController> bars = new List<XylophoneBarController>();

        private void Reset()
        {
            instrumentType = InstrumentType.Xylophone;
            instrumentDisplayName = "Xylophone";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Xylophone;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Xylophone";

            base.Initialize(nav);

            bars.RemoveAll(b => b == null);
            if (bars.Count == 0)
            {
                bars.AddRange(GetComponentsInChildren<XylophoneBarController>(true));
            }
        }
    }
}
