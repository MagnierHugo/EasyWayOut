
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ShotgunRound : MonoBehaviour
{
    [field: SerializeField] public bool IsLive { get; private set; }
    [SerializeField] private GameObject textGameObject;
    [SerializeField] private float expandingSpeed = 3f;
    private static Vector3 initialSize;
    [SerializeField] private float maxSizeCoefficient = 1.5f;
    private void Awake() => initialSize = transform.localScale;

    private bool hovered;
    private void Update()
    {
        Vector3 maxSize = initialSize * maxSizeCoefficient;
        if (hovered)
        {
            if (!textGameObject.activeSelf)
                textGameObject.SetActive(true);

            if (transform.localScale == maxSize)
                return;

            transform.localScale = Vector3.Lerp(transform.localScale, maxSize, expandingSpeed * Time.deltaTime);
        }
        else
        {
            if (textGameObject.activeSelf)
                textGameObject.SetActive(false);

            transform.localScale = initialSize;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnHoverEnter() => hovered = true;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void OnHoverExit() => hovered = false;


}
