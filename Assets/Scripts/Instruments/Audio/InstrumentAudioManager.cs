using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class InstrumentAudioManager : MonoBehaviour
    {
        public static InstrumentAudioManager Instance { get; private set; }

        [Header("Audio Settings")]
        [SerializeField] private int poolSize = 20;
        [SerializeField] [Range(0f, 1f)] private float masterVolume = 0.9f;

        private readonly List<AudioSource> audioSourcePool = new List<AudioSource>();
        private int currentSourceIndex = 0;
        private bool isMuted = false;

        public bool IsMuted => isMuted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializePool();

            isMuted = PlayerPrefs.GetInt("Instrument_Muted", 0) == 1;
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource src = gameObject.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.spatialBlend = 0f; // 2D Sound
                src.rolloffMode = AudioRolloffMode.Linear;
                audioSourcePool.Add(src);
            }
        }

        public AudioSource PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null || isMuted) return null;

            AudioSource source = GetNextSource();
            source.clip = clip;
            source.volume = Mathf.Clamp01(volume * masterVolume);
            source.pitch = Mathf.Clamp(pitch, 0.2f, 3f);
            source.loop = false;
            source.Play();
            return source;
        }

        public AudioSource PlayLooping(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null || isMuted) return null;

            AudioSource source = GetNextSource();
            source.clip = clip;
            source.volume = Mathf.Clamp01(volume * masterVolume);
            source.pitch = Mathf.Clamp(pitch, 0.2f, 3f);
            source.loop = true;
            source.Play();
            return source;
        }

        public void StopSource(AudioSource source, float fadeDuration = 0.05f)
        {
            if (source == null) return;
            if (fadeDuration <= 0f)
            {
                source.Stop();
            }
            else
            {
                StartCoroutine(FadeOutAndStop(source, fadeDuration));
            }
        }

        private System.Collections.IEnumerator FadeOutAndStop(AudioSource source, float duration)
        {
            float startVol = source.volume;
            float elapsed = 0f;
            while (elapsed < duration && source != null && source.isPlaying)
            {
                elapsed += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(startVol, 0f, elapsed / duration);
                yield return null;
            }
            if (source != null)
            {
                source.Stop();
                source.volume = startVol;
            }
        }

        public void ToggleMute()
        {
            isMuted = !isMuted;
            PlayerPrefs.SetInt("Instrument_Muted", isMuted ? 1 : 0);
            PlayerPrefs.Save();

            if (isMuted)
            {
                foreach (var src in audioSourcePool)
                {
                    if (src.isPlaying) src.Stop();
                }
            }
        }

        private AudioSource GetNextSource()
        {
            // Find an idle source
            for (int i = 0; i < audioSourcePool.Count; i++)
            {
                int idx = (currentSourceIndex + i) % audioSourcePool.Count;
                if (!audioSourcePool[idx].isPlaying)
                {
                    currentSourceIndex = (idx + 1) % audioSourcePool.Count;
                    return audioSourcePool[idx];
                }
            }

            // If all are busy, cycle through circular buffer
            AudioSource recycled = audioSourcePool[currentSourceIndex];
            currentSourceIndex = (currentSourceIndex + 1) % audioSourcePool.Count;
            return recycled;
        }
    }
}
