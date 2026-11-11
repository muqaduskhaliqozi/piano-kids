using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PianoKids.Instruments
{
    public class GuitarController : InstrumentGameplayPanel
    {
        [Header("Guitar Strings")]
        [SerializeField] private List<GuitarStringController> strings = new List<GuitarStringController>();

        [Header("Chord Buttons")]
        [SerializeField] private Button chordCButton;
        [SerializeField] private Button chordGButton;
        [SerializeField] private Button chordAmButton;
        [SerializeField] private Button chordFButton;

        [Header("Chord Audio Clips")]
        [SerializeField] private AudioClip chordCClip;
        [SerializeField] private AudioClip chordGClip;
        [SerializeField] private AudioClip chordAmClip;
        [SerializeField] private AudioClip chordFClip;

        private void Reset()
        {
            instrumentType = InstrumentType.Guitar;
            instrumentDisplayName = "Guitar";
        }

        public override void Initialize(InstrumentsNavigationController nav)
        {
            instrumentType = InstrumentType.Guitar;
            if (string.IsNullOrEmpty(instrumentDisplayName))
                instrumentDisplayName = "Guitar";

            base.Initialize(nav);

            strings.RemoveAll(s => s == null);
            if (strings.Count == 0)
            {
                strings.AddRange(GetComponentsInChildren<GuitarStringController>(true));
            }

            if (chordCButton != null)
            {
                chordCButton.onClick.RemoveAllListeners();
                chordCButton.onClick.AddListener(() => PlayChord(chordCClip, chordCButton.transform));
            }
            if (chordGButton != null)
            {
                chordGButton.onClick.RemoveAllListeners();
                chordGButton.onClick.AddListener(() => PlayChord(chordGClip, chordGButton.transform));
            }
            if (chordAmButton != null)
            {
                chordAmButton.onClick.RemoveAllListeners();
                chordAmButton.onClick.AddListener(() => PlayChord(chordAmClip, chordAmButton.transform));
            }
            if (chordFButton != null)
            {
                chordFButton.onClick.RemoveAllListeners();
                chordFButton.onClick.AddListener(() => PlayChord(chordFClip, chordFButton.transform));
            }
        }

        private void PlayChord(AudioClip clip, Transform btnTransform)
        {
            if (InstrumentAudioManager.Instance != null && clip != null)
            {
                InstrumentAudioManager.Instance.PlaySound(clip, 1f);
            }

            if (btnTransform != null)
            {
                StartCoroutine(PunchScale(btnTransform, new Vector3(1.18f, 1.18f, 1f), 0.16f));
            }

            // Also vibrate all strings together
            foreach (var str in strings)
            {
                if (str != null) str.Pluck();
            }
        }
    }
}
