using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

[System.Serializable]
public struct TextItem 
{
    [TextArea]
    public string Text; 
    public float Duration;
}

public class CinematicText : MonoBehaviour 
{
    [SerializeField] private List<TextItem> inspectorTexts = new List<TextItem>();
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Image displayBlur;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float fallDistance = 25f;     
    [SerializeField] private float delayBetweenTexts = 0.5f;
    [SerializeField] private float maxBlurForce = 1f; 

    private Queue<TextItem> textsToDisplay = new Queue<TextItem>();
    private RectTransform textRectTransform;
    private Vector2 originalPosition;
    private Material blurMaterialInstance;
    
    // Store the original alpha of the blur image so we know what to fade it up to
    private float originalBlurAlpha = 1f; 

    void Start() 
    {
        textRectTransform = displayText.GetComponent<RectTransform>();
        originalPosition = textRectTransform.anchoredPosition;

        if (displayBlur != null)
        {
            originalBlurAlpha = displayBlur.color.a;

            if (displayBlur.material != null)
            {
                blurMaterialInstance = new Material(displayBlur.material);
                displayBlur.material = blurMaterialInstance;
                blurMaterialInstance.SetFloat("_BlurForce", 0f);
            }
        }

        UpdateDisplaySequence(inspectorTexts);

        if (textsToDisplay.Count > 0) 
        {
            StartDisplaySequence();
        }
    }

    public void UpdateDisplaySequence(List<TextItem> Sequences) 
    {
        foreach (TextItem item in Sequences) 
        {
            textsToDisplay.Enqueue(item);
        }
    }

    public void StartDisplaySequence() 
    {
        StartCoroutine(DisplaySequence());
    }

    private IEnumerator DisplaySequence() 
    {
        while (textsToDisplay.Count > 0) 
        {
            TextItem currentItem = textsToDisplay.Dequeue();

            // 1. Reset text position, set the new text, and force Alpha to 0
            textRectTransform.anchoredPosition = originalPosition;
            displayText.text = currentItem.Text;
            displayText.alpha = 0f; // <-- Using TMP's dedicated alpha property
            
            // Set initial Blur image transparency to 0
            if (displayBlur != null)
            {
                Color blurColor = displayBlur.color;
                blurColor.a = 0f;
                displayBlur.color = blurColor;
            }

            // --- FADE IN ---
            float timer = 0f;
            while (timer < fadeInDuration) 
            {
                timer += Time.deltaTime;
                float progress = timer / fadeInDuration;

                // Text fades in directly via .alpha
                displayText.alpha = Mathf.Lerp(0f, 1f, progress);

                if (displayBlur != null)
                {
                    // Fade Blur transparency
                    Color blurColor = displayBlur.color;
                    blurColor.a = Mathf.Lerp(0f, originalBlurAlpha, progress);
                    displayBlur.color = blurColor;

                    // Increase Blur Force
                    if (blurMaterialInstance != null)
                    {
                        blurMaterialInstance.SetFloat("_BlurForce", Mathf.Lerp(0f, maxBlurForce, progress));
                    }
                }

                yield return null;
            }

            displayText.alpha = 1f;
            yield return new WaitForSeconds(currentItem.Duration);

            // --- FADE OUT ---
            timer = 0f;
            Vector2 targetPosition = originalPosition - new Vector2(0, fallDistance);

            while (timer < fadeOutDuration) 
            {
                timer += Time.deltaTime;
                float progress = timer / fadeOutDuration;

                // Text fades out directly via .alpha and falls
                displayText.alpha = Mathf.Lerp(1f, 0f, progress);
                textRectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, progress);

                if (displayBlur != null)
                {
                    // Fade Blur transparency
                    Color blurColor = displayBlur.color;
                    blurColor.a = Mathf.Lerp(originalBlurAlpha, 0f, progress);
                    displayBlur.color = blurColor;

                    // Decrease Blur Force
                    if (blurMaterialInstance != null)
                    {
                        blurMaterialInstance.SetFloat("_BlurForce", Mathf.Lerp(maxBlurForce, 0f, progress));
                    }
                }

                yield return null; 
            }

            displayText.alpha = 0f;
            if (displayBlur != null)
            {
                Color finalBlurColor = displayBlur.color;
                finalBlurColor.a = 0f;
                displayBlur.color = finalBlurColor;
            }

            yield return new WaitForSeconds(delayBetweenTexts);
        }
        
        displayText.text = "";
    }
}