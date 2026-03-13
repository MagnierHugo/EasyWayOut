
using UnityEngine;

public sealed class ShotgunRound : MonoBehaviour
{
    [field: SerializeField] public bool IsLive { get; private set; }
    [SerializeField] public GameObject textGameObject;
    [SerializeField] private float expandingSpeed = 3f;
    private static Vector3 initialSize;
    [SerializeField] private Vector3 maxSize;
    private void Awake() => initialSize = transform.localScale;

    private bool hovered;
    private void Update()
    {
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
    public void OnHoverEnter() => hovered = true;
    public void OnHoverExit() => hovered = false;


}
