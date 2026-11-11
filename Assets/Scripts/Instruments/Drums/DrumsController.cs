using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class DrumsController : InstrumentGameplayPanel
    {
        [Header("Drum Pads")]
        [SerializeField] private DrumPadController bassDrum;
        [SerializeField] private DrumPadController snareDrum;
        [SerializeField] private DrumPadController hiHat;
        [SerializeField] private DrumPadController floorTom;
        [SerializeField] private DrumPadController highTom;
        [SerializeField] private DrumPadController cymbal;

        private void Reset()
        {
            instrumentType = InstrumentType.Drums;
            instrumentDisplayName = "Drums";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Drums;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Drums";

            base.Initialize(nav);

            if (bassDrum == null) bassDrum = transform.Find("DrumKit/BassDrum")?.GetComponent<DrumPadController>();
            if (snareDrum == null) snareDrum = transform.Find("DrumKit/SnareDrum")?.GetComponent<DrumPadController>();
            if (hiHat == null) hiHat = transform.Find("DrumKit/HiHat")?.GetComponent<DrumPadController>();
            if (floorTom == null) floorTom = transform.Find("DrumKit/FloorTom")?.GetComponent<DrumPadController>();
            if (highTom == null) highTom = transform.Find("DrumKit/HighTom")?.GetComponent<DrumPadController>();
            if (cymbal == null) cymbal = transform.Find("DrumKit/Cymbal")?.GetComponent<DrumPadController>();
        }
    }
}
