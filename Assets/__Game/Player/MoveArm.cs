using UnityEngine;
using UnityEngine.InputSystem;

public class MoveArm : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private GameObject armPivot = null;
    [SerializeField] private GameObject handPivot = null;

    private float minArmRotation = -50f;
    private float maxArmRotation = 10f;
    private float minHandRotation = -25f;
    private float maxHandRotation = 30f;

    private float stepSize = 5f;
    private float currentAngleX = 0f;
    private float currentHandAngleX = 0f;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Scroll.performed += OnScrollPerformed;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Scroll.performed -= OnScrollPerformed;
        controls.Disable();
    }

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        float scrollDelta = context.ReadValue<Vector2>().y;

        if (scrollDelta != 0)
        {
            float direction = -Mathf.Sign(scrollDelta);

            currentAngleX += stepSize * direction;

            currentHandAngleX += stepSize * direction;


            currentAngleX = Mathf.Clamp(currentAngleX, minArmRotation, maxArmRotation);

            armPivot.transform.localRotation = Quaternion.Euler(currentAngleX, 0, 0);


            currentHandAngleX = Mathf.Clamp(currentAngleX, minHandRotation, maxHandRotation);

            handPivot.transform.localRotation = Quaternion.Euler(currentHandAngleX, 0, 0);
        }
    }
}
