using UnityEngine;
using System.Collections;
using UnityEngine.Rendering; // Required for Post Processing Volume



public class LightingManager : MonoBehaviour
{
    [Header("References")]
    public Light tableSpotlight;
    public Volume postProcessVolume;

    [Header("Round Settings")]
    public float transitionDuration = 2f;
    public RoundLighting[] rounds;

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