using UnityEngine;

[System.Serializable]
public class RoundLighting
{
    [Tooltip("The color of the spotlight")]
    public Color lightColor = Color.white;
    [Tooltip("How bright the spotlight is")]
    public float intensity = 10f;
    [Tooltip("How wide the circle of light is")]
    public float spotAngle = 45f;
    [Tooltip("Enable for the final round to make the light buzz/flicker")]
    public bool enableFlicker = false;
    [Tooltip("Enable to turn on the heavy post-processing effects")]
    public bool enableInsanityPostProcess = false;
}