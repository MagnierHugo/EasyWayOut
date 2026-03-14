using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UI;

public sealed class FadeEffect : MonoBehaviour
{
    private Image image;
    private static FadeEffect instance;
    private void Awake()
    {
        image = GetComponent<Image>();
        instance = this;
        SetAlpha(0);
    }

    public static IEnumerator Fade(float inDuration, float duration, float outDuration)
    {
        float elapsed = 0;
        while (elapsed < inDuration)
        {
            instance.SetAlpha(Mathf.Lerp(0, 1, elapsed / inDuration));

            yield return elapsed += Time.deltaTime;
        }

        instance.SetAlpha(1);

        yield return new WaitForSecondsRealtime(duration);

        elapsed = 0;
        while (elapsed < outDuration)
        {
            instance.SetAlpha(Mathf.Lerp(1, 0, elapsed / outDuration));

            yield return elapsed += Time.deltaTime;
        }

        instance.SetAlpha(0);
    }

    private void SetAlpha(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    


}
