
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PianoKids.Instruments
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class FloatingStarUI : MonoBehaviour
    {
        private RectTransform starRect;
        private Image starImage;
        private Coroutine animationRoutine;

        private void Awake()
        {
            starRect = GetComponent<RectTransform>();
            starImage = GetComponent<Image>();
        }

        public void Play(
            Sprite sprite,
            Color tint,
            Vector2 direction,
            float distance,
            float targetSize,
            float rotationSpeed,
            float duration)
        {
            if (starRect == null)
                starRect = GetComponent<RectTransform>();

            if (starImage == null)
                starImage = GetComponent<Image>();

            if (animationRoutine != null)
                StopCoroutine(animationRoutine);

            starImage.sprite = sprite;
            starImage.color = new Color(tint.r, tint.g, tint.b, 1f);

            starRect.sizeDelta = new Vector2(targetSize, targetSize);
            starRect.localScale = Vector3.one * 0.15f;

            animationRoutine = StartCoroutine(AnimateStar(
                tint,
                direction.normalized,
                distance,
                rotationSpeed,
                duration
            ));
        }

        private IEnumerator AnimateStar(
            Color tint,
            Vector2 direction,
            float distance,
            float rotationSpeed,
            float duration)
        {
            Vector2 startPosition = starRect.anchoredPosition;
            float elapsed = 0f;

            float startAngle = starRect.localEulerAngles.z;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                float t = Mathf.Clamp01(elapsed / duration);

                // Move outward in burst direction
                float movement = Mathf.SmoothStep(0f, 1f, t);

                starRect.anchoredPosition =
                    startPosition + direction * distance * movement;

                // Grow first, then shrink
                float scale;

                if (t < 0.3f)
                {
                    scale = Mathf.Lerp(0.15f, 1f, t / 0.3f);
                }
                else
                {
                    scale = Mathf.Lerp(1f, 0.1f, (t - 0.3f) / 0.7f);
                }

                starRect.localScale = Vector3.one * scale;

                // Rotate while moving
                starRect.localRotation = Quaternion.Euler(
                    0f,
                    0f,
                    startAngle + rotationSpeed * elapsed
                );

                // Fade out
                float alpha = 1f - t;

                starImage.color = new Color(
                    tint.r,
                    tint.g,
                    tint.b,
                    alpha
                );

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
