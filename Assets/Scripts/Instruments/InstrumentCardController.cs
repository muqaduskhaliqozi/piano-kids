using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class InstrumentCardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [Header("Instrument Data")]
        [SerializeField] private InstrumentType instrumentType;
        [SerializeField] private string instrumentName;

        [Header("UI References")]
        [SerializeField] private Image cardBackground;
        [SerializeField] private Image instrumentIcon;
        [SerializeField] private TMP_Text instrumentNameText;
        [SerializeField] private Button selectButton;

        public event Action<InstrumentType> OnCardClicked;

        private Vector3 initialScale = Vector3.one;
        private Coroutine scaleCoroutine;

        public InstrumentType Type => instrumentType;
        public string InstrumentName => instrumentName;

        private void Awake()
        {
            initialScale = transform.localScale;
            if (selectButton == null) selectButton = GetComponent<Button>();

            if (selectButton != null)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.onClick.AddListener(HandleCardClick);
            }
        }

        public void Setup(InstrumentType type, string name, Sprite icon, Color bgColor, Action<InstrumentType> clickCallback)
        {
            instrumentType = type;
            instrumentName = name;

            if (instrumentNameText != null)
                instrumentNameText.text = name;

            if (instrumentIcon != null && icon != null)
                instrumentIcon.sprite = icon;

            if (cardBackground != null)
                cardBackground.color = bgColor;

            OnCardClicked = clickCallback;
        }

        public void HandleCardClick()
        {
            AnimateScale(initialScale * 0.9f, 0.08f);

            if (OnCardClicked != null)
            {
                OnCardClicked.Invoke(instrumentType);
            }
            else
            {
                var nav = FindObjectOfType<InstrumentsNavigationController>();
                if (nav != null)
                {
                    nav.OpenInstrument(instrumentType);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateScale(initialScale * 0.92f, 0.08f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            AnimateScale(initialScale, 0.12f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // If selectButton is attached, its Button.onClick fires HandleCardClick
            // If no selectButton component exists, fire it here
            if (selectButton == null)
            {
                HandleCardClick();
            }
        }

        private void AnimateScale(Vector3 targetScale, float duration)
        {
            if (!gameObject.activeInHierarchy) return;
            if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleRoutine(targetScale, duration));
        }

        private IEnumerator ScaleRoutine(Vector3 target, float duration)
        {
            Vector3 start = transform.localScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(start, target, elapsed / duration);
                yield return null;
            }
            transform.localScale = target;
        }
    }
}
