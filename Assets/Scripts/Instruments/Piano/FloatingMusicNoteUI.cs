
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PianoKids.Instruments
{
    public class FloatingMusicNoteUI : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float duration = 0.9f;
        [SerializeField] private float moveUp = 120f;
        [SerializeField] private float horizontalMove = 35f;
        [SerializeField] private float rotationAmount = 25f;

        private RectTransform rect;
        private Image noteImage;
        private Vector2 startPosition;
        private Color startColor;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            noteImage = GetComponent<Image>();
        }

        public void Play(Sprite sprite, Color tint)
        {
            if (sprite != null)
                noteImage.sprite = sprite;

            startColor = tint;
            startColor.a = 1f;
            noteImage.color = startColor;

            startPosition = rect.anchoredPosition;

            StartCoroutine(AnimateNote());
        }

        private IEnumerator AnimateNote()
        {
            float elapsed = 0f;

            Vector2 movement = new Vector2(
                Random.Range(-horizontalMove, horizontalMove),
                moveUp
            );

            float rotation = Random.Range(-rotationAmount, rotationAmount);

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                float t = Mathf.Clamp01(elapsed / duration);
                float smooth = Mathf.SmoothStep(0f, 1f, t);

                rect.anchoredPosition =
                    startPosition + movement * smooth;

                rect.localScale = Vector3.one *
                    Mathf.Lerp(0.65f, 1.15f, smooth);

                rect.localRotation = Quaternion.Euler(
                    0f, 0f, rotation * smooth
                );

                Color color = startColor;
                color.a = 1f - smooth;
                noteImage.color = color;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
