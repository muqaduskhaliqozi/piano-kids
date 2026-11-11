
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace PianoKids.Instruments
{
    public class PianoKeyController : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [Header("Note Setup")]
        [SerializeField] private string noteName = "C4";
        [SerializeField] private AudioClip noteClip;
        [SerializeField] private bool isBlackKey = false;

        [Header("Visual References")]
        [SerializeField] private Image keyImage;
        [SerializeField] private TMP_Text noteLabel;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite pressedSprite;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField]
        private Color pressedColor =
            new Color(0.85f, 0.92f, 1f, 1f);

        [Header("Press Effects")]
        [SerializeField] private bool enableMusicNoteEffect = true;

        private AudioSource activeVoice = null;
        private Vector3 initialPosition;
        private bool isPressed = false;

        public string NoteName => noteName;
        public bool IsBlackKey => isBlackKey;

        private void Awake()
        {
            if (keyImage == null)
                keyImage = GetComponent<Image>();

            initialPosition = transform.localPosition;

            UpdateVisuals(false);
        }

        public void Setup(
            string note,
            AudioClip clip,
            bool blackKey,
            Sprite normal,
            Sprite pressed)
        {
            noteName = note;
            noteClip = clip;
            isBlackKey = blackKey;
            normalSprite = normal;
            pressedSprite = pressed;

            if (noteLabel != null)
                noteLabel.text = note.Replace("b", "♭");

            if (isBlackKey)
            {
                normalColor = new Color(0.18f, 0.2f, 0.28f);
                pressedColor = new Color(0.35f, 0.45f, 0.65f);
            }
            else
            {
                normalColor = Color.white;
                pressedColor = new Color(0.85f, 0.92f, 1f);
            }

            UpdateVisuals(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PressKey();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ReleaseKey();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Glissando / slide support
            if (eventData.dragging ||
                Input.GetMouseButton(0) ||
                Input.touchCount > 0)
            {
                PressKey();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPressed)
                ReleaseKey();
        }

        private void PressKey()
        {
            if (isPressed)
                return;

            isPressed = true;

            // Play piano note
            if (InstrumentAudioManager.Instance != null &&
                noteClip != null)
            {
                activeVoice =
                    InstrumentAudioManager.Instance.PlaySound(noteClip, 1f);
            }

            // Press visual
            UpdateVisuals(true);

            transform.localPosition =
                initialPosition + new Vector3(0, -6f, 0);

            // Spawn floating music note effect
            if (enableMusicNoteEffect)
            {
                FloatingMusicNoteEffectSpawner.SpawnFor(
                    GetComponent<RectTransform>()
                );
            }
        }

        private void ReleaseKey()
        {
            if (!isPressed)
                return;

            isPressed = false;

            // Stop piano note smoothly
            if (activeVoice != null &&
                InstrumentAudioManager.Instance != null)
            {
                InstrumentAudioManager.Instance.StopSource(
                    activeVoice,
                    0.12f
                );

                activeVoice = null;
            }

            // Restore normal visual
            UpdateVisuals(false);

            transform.localPosition = initialPosition;
        }

        private void UpdateVisuals(bool pressed)
        {
            if (keyImage == null)
                return;

            if (pressed && pressedSprite != null)
            {
                keyImage.sprite = pressedSprite;
            }
            else if (!pressed && normalSprite != null)
            {
                keyImage.sprite = normalSprite;
            }

            keyImage.color = pressed ? pressedColor : normalColor;
        }

        private void OnDisable()
        {
            if (isPressed)
                ReleaseKey();
        }
    }
}
