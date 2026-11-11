using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PianoKids.Instruments
{
    public abstract class InstrumentGameplayPanel : MonoBehaviour
    {
        [Header("Instrument Panel Setup")]
        [SerializeField] protected InstrumentType instrumentType;
        [SerializeField] protected string instrumentDisplayName;

        [Header("Common UI References")]
        [SerializeField] protected Button backButton;
        [SerializeField] protected TMP_Text titleText;
        [SerializeField] protected Button soundToggleButton;
        [SerializeField] protected Image soundToggleIcon;

        [Header("Visual References")]
        [SerializeField] protected Sprite soundOnSprite;
        [SerializeField] protected Sprite soundOffSprite;

        protected InstrumentsNavigationController navigationController;

        public InstrumentType Type => instrumentType;
        public string DisplayName => instrumentDisplayName;

        public virtual void Initialize(InstrumentsNavigationController nav)
        {
            navigationController = nav;

            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(OnBackClicked);
            }

            if (soundToggleButton != null)
            {
                soundToggleButton.onClick.RemoveAllListeners();
                soundToggleButton.onClick.AddListener(OnSoundToggleClicked);
                UpdateSoundIcon();
            }

            if (titleText != null && !string.IsNullOrEmpty(instrumentDisplayName))
            {
                titleText.text = instrumentDisplayName.ToUpper();
            }
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            UpdateSoundIcon();
            StopAllCoroutines();
            StartCoroutine(AnimateOpen());
        }

        public virtual void Close()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        protected virtual void OnBackClicked()
        {
            if (navigationController != null)
            {
                navigationController.ReturnToSelection();
            }
        }

        protected virtual void OnSoundToggleClicked()
        {
            if (InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.ToggleMute();
                UpdateSoundIcon();
            }
        }

        protected void UpdateSoundIcon()
        {
            if (soundToggleIcon != null && InstrumentAudioManager.Instance != null)
            {
                soundToggleIcon.color = InstrumentAudioManager.Instance.IsMuted
                    ? new Color(0.7f, 0.7f, 0.7f, 0.6f)
                    : Color.white;
            }
        }

        protected virtual IEnumerator AnimateOpen()
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                float duration = 0.2f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    cg.alpha = Mathf.Clamp01(elapsed / duration);
                    yield return null;
                }
                cg.alpha = 1f;
            }
        }

        public static IEnumerator PunchScale(Transform target, Vector3 punchScale, float duration)
        {
            if (target == null) yield break;
            Vector3 originalScale = Vector3.one;
            float half = duration * 0.4f;
            float elapsed = 0f;

            while (elapsed < half && target != null)
            {
                elapsed += Time.unscaledDeltaTime;
                target.localScale = Vector3.Lerp(originalScale, punchScale, elapsed / half);
                yield return null;
            }

            elapsed = 0f;
            float remain = duration - half;
            while (elapsed < remain && target != null)
            {
                elapsed += Time.unscaledDeltaTime;
                target.localScale = Vector3.Lerp(punchScale, originalScale, elapsed / remain);
                yield return null;
            }

            if (target != null) target.localScale = originalScale;
        }
    }
}
