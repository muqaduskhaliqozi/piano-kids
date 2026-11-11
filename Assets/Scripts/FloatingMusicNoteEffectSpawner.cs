
using System.Collections.Generic;
using UnityEngine;

namespace PianoKids.Instruments
{
    public class FloatingMusicNoteEffectSpawner : MonoBehaviour
    {
        private static readonly List<FloatingMusicNoteEffectSpawner> spawners =
            new List<FloatingMusicNoteEffectSpawner>();

        [Header("References")]
        [SerializeField] private RectTransform effectsLayer;
        [SerializeField] private FloatingMusicNoteUI notePrefab;

        [Header("Music Note Sprites")]
        [SerializeField] private Sprite[] noteSprites;

        [Header("Spawn Settings")]
        [SerializeField] private Vector2 spawnOffset = new Vector2(0, 25);
        [SerializeField] private float randomOffset = 20f;

        private void OnEnable()
        {
            if (!spawners.Contains(this))
                spawners.Add(this);
        }

        private void OnDisable()
        {
            spawners.Remove(this);
        }

        // Automatically finds the EffectsLayer closest to the pressed control.
        public static void SpawnFor(RectTransform source)
        {
            if (source == null)
                return;

            FloatingMusicNoteEffectSpawner bestSpawner = null;
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
                bestSpawner.SpawnFrom(source);
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

        private void SpawnFrom(RectTransform source)
        {
            if (source == null ||
                notePrefab == null ||
                effectsLayer == null ||
                noteSprites == null ||
                noteSprites.Length == 0)
                return;

            FloatingMusicNoteUI note =
                Instantiate(notePrefab, effectsLayer, false);

            RectTransform noteRect =
                note.GetComponent<RectTransform>();

            Vector3 offset = new Vector3(
                Random.Range(-randomOffset, randomOffset),
                spawnOffset.y + Random.Range(-randomOffset, randomOffset),
                0
            );

            noteRect.position = source.position + offset;

            Sprite selectedSprite =
                noteSprites[Random.Range(0, noteSprites.Length)];

            Color[] colors =
            {
                new Color(1f, 0.35f, 0.65f),
                new Color(0.25f, 0.75f, 1f),
                new Color(1f, 0.8f, 0.2f),
                new Color(0.65f, 0.4f, 1f),
                Color.white
            };

            Color selectedColor =
                colors[Random.Range(0, colors.Length)];

            note.Play(selectedSprite, selectedColor);
        }
    }
}
