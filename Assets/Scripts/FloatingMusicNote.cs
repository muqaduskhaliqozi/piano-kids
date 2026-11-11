
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingMusicNote : MonoBehaviour
{
    public float moveDistance = 100f;
    public float duration = 0.8f;
    public float horizontalMovement = 30f;

    private RectTransform rect;
    private Image image;
    private Vector2 startPosition;
    private Color startColor;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Play()
    {
        startPosition = rect.anchoredPosition;
        startColor = image.color;

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float timer = 0f;

        Vector2 direction = new Vector2(
            Random.Range(-horizontalMovement, horizontalMovement),
            moveDistance
        );

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            rect.anchoredPosition =
                startPosition + direction * t;

            float scale = Mathf.Lerp(0.5f, 1.2f, t);

            rect.localScale = Vector3.one * scale;

            Color c = startColor;
            c.a = 1f - t;
            image.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}
