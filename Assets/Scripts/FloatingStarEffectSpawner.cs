
using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class FloatingStarEffectSpawner : MonoBehaviour
    {
        private static readonly List<FloatingStarEffectSpawner> spawners =
            new List<FloatingStarEffectSpawner>();

        [Header("References")]
        [SerializeField] private RectTransform effectsLayer;
        [SerializeField] private FloatingStarUI starPrefab;

        [Header("Star Sprites")]
        [SerializeField] private Sprite[] starSprites;

        [Header("Star Colors")]
        [SerializeField]
        private Color[] starColors =
        {
            new Color(1f, 0.85f, 0.15f),
            new Color(1f, 0.4f, 0.65f),
            new Color(0.3f, 0.75f, 1f),
            new Color(0.7f, 0.45f, 1f),
            Color.white
        };

        [Header("Burst Settings")]
        [SerializeField] private int starsPerPress = 4;
        [SerializeField] private float minDistance = 45f;
        [SerializeField] private float maxDistance = 85f;
        [SerializeField] private float minSize = 22f;
        [SerializeField] private float maxSize = 42f;
        [SerializeField] private float animationDuration = 0.55f;
        [SerializeField] private float angleRandomness = 18f;

        private void OnEnable()
        {
            if (!spawners.Contains(this))
                spawners.Add(this);
        }

        private void OnDisable()
        {
            spawners.Remove(this);
        }

        public static void SpawnFor(RectTransform source)
        {
            if (source == null)
                return;

            FloatingStarEffectSpawner bestSpawner = null;
            int bestDistance = int.MaxValue;

            foreach (var spawner in spawners)
            {
                if (spawner == null ||
                    !spawner.isActiveAndEnabled ||
                    spawner.effectsLayer == null)
                    continue;

                int distance = GetHierarchyDistance(
                    source,
                    spawner.effectsLayer
                );

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestSpawner = spawner;
                }
            }

            if (bestSpawner != null)
                bestSpawner.SpawnStars(source);
        }

        private static int GetHierarchyDistance(
            Transform first,
            Transform second)
        {
            Dictionary<Transform, int> ancestors =
                new Dictionary<Transform, int>();

            int distance = 0;
            Transform current = first;

            while (current != null)
            {
                ancestors[current] = distance;
                distance++;
                current = current.parent;
            }

            distance = 0;
            current = second;

            while (current != null)
            {
                if (ancestors.TryGetValue(current, out int firstDistance))
                    return firstDistance + distance;

                distance++;
                current = current.parent;
            }

            return int.MaxValue;
        }

        private void SpawnStars(RectTransform source)
        {
            if (source == null ||
                effectsLayer == null ||
                starPrefab == null ||
                starSprites == null ||
                starSprites.Length == 0 ||
                starColors == null ||
                starColors.Length == 0)
                return;

            Canvas canvas = effectsLayer.GetComponentInParent<Canvas>();

            Camera uiCamera = null;

            if (canvas != null &&
                canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                uiCamera = canvas.worldCamera;
            }

            Vector2 screenPosition =
                RectTransformUtility.WorldToScreenPoint(
                    uiCamera,
                    source.position
                );

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                effectsLayer,
                screenPosition,
                uiCamera,
                out Vector2 localPosition))
            {
                return;
            }

            int count = Mathf.Max(1, starsPerPress);

            // Random starting angle for the burst
            float baseAngle = Random.Range(0f, 360f);

            for (int i = 0; i < count; i++)
            {
                FloatingStarUI star = Instantiate(
                    starPrefab,
                    effectsLayer,
                    false
                );

                RectTransform starRect =
                    star.GetComponent<RectTransform>();

                starRect.anchoredPosition = localPosition;

                // Spread stars in different directions
                float angle = baseAngle +
                    (360f / count) * i +
                    Random.Range(-angleRandomness, angleRandomness);

                float radians = angle * Mathf.Deg2Rad;

                Vector2 direction = new Vector2(
                    Mathf.Cos(radians),
                    Mathf.Sin(radians)
                );

                Sprite selectedSprite = starSprites[
                    Random.Range(0, starSprites.Length)
                ];

                Color selectedColor = starColors[
                    Random.Range(0, starColors.Length)
                ];

                float distance = Random.Range(
                    minDistance,
                    maxDistance
                );

                float size = Random.Range(
                    minSize,
                    maxSize
                );

                float rotationSpeed = Random.Range(180f, 540f);

                if (Random.value > 0.5f)
                    rotationSpeed *= -1f;

                star.Play(
                    selectedSprite,
                    selectedColor,
                    direction,
                    distance,
                    size,
                    rotationSpeed,
                    animationDuration
                );
            }
        }
    }
}
