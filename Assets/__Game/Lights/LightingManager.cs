using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light tableSpotlight;
    [SerializeField] private Volume postProcessVolume;

    [Header("Round Settings")]
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private RoundLighting[] rounds;

    [Header("Round Settings")]
    [SerializeField] private MeshRenderer tableRenderer = null;
    [SerializeField] private Material[] tableMaterials = null;


    private Coroutine currentTransition;
    private bool isFlickering = false;
    private float baseIntensity;

    // Call this function when a new opponent sits down
    // Example: ChangeRound(0) for Round 1, ChangeRound(5) for Round 6
    public void ChangeRound(int roundIndex)
    {
        if (roundIndex < 0 || roundIndex >= rounds.Length) return;

        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(TransitionLight(rounds[roundIndex]));

        UpdateTableMaterial(roundIndex);
    }

    private IEnumerator TransitionLight(RoundLighting target)
    {
        isFlickering = false;
        float timeElapsed = 0;

        Color startColor = tableSpotlight.color;
        float startIntensity = tableSpotlight.intensity;
        float startAngle = tableSpotlight.spotAngle;

        while (timeElapsed < transitionDuration)
        {
            tableSpotlight.color = Color.Lerp(startColor, target.lightColor, timeElapsed / transitionDuration);
            tableSpotlight.intensity = Mathf.Lerp(startIntensity, target.intensity, timeElapsed / transitionDuration);
            tableSpotlight.spotAngle = Mathf.Lerp(startAngle, target.spotAngle, timeElapsed / transitionDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        tableSpotlight.color = target.lightColor;
        tableSpotlight.intensity = target.intensity;
        tableSpotlight.spotAngle = target.spotAngle;

        baseIntensity = target.intensity;
        isFlickering = target.enableFlicker;

        if (postProcessVolume != null)
        {
            postProcessVolume.weight = target.enableInsanityPostProcess ? 1f : 0f;
        }
    }

    private void UpdateTableMaterial(int roundIndex)
    {
        // 1. Make sure we actually have materials assigned and the index is valid
        if (tableMaterials != null && roundIndex < tableMaterials.Length)
        {
            // 2. Swap the material directly on the renderer
            tableRenderer.material = tableMaterials[roundIndex];
        }
        else
        {
            Debug.LogWarning("Tried to change table material, but the index was out of bounds or materials array is empty.");
        }
    }

    private void Update()
    {
        if (isFlickering)
        {
            if (Random.value < 0.15f)
            {
                tableSpotlight.intensity = Random.Range(baseIntensity * 0.2f, baseIntensity * 0.6f);
            }
            else
            {
                tableSpotlight.intensity = baseIntensity;
            }
        }
    }
}