using UnityEngine;
using UnityEngine.InputSystem;

public class MoveArm : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private Player player = null;
    [SerializeField] private GameObject armPivot = null;
    [SerializeField] private GameObject handPivot = null;

    private Vector3 gunPickupPos = new Vector3(.088f, .2f, 0f);
    private Quaternion gunPickupRot = Quaternion.Euler(0f, 0f, 7.13f);

    private float minArmRotation = -50f;
    private float maxArmRotation = 10f;
    private float minHandRotation = -45f;
    private float maxHandRotation = 45f;

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

            currentHandAngleX += stepSize * 1.5f * direction;


            currentAngleX = Mathf.Clamp(currentAngleX, minArmRotation, maxArmRotation);

            armPivot.transform.localRotation = Quaternion.Euler(currentAngleX, 0, 0);

            if (currentAngleX >= maxArmRotation -1) { ChangeTarget(true, true); }
            else if (currentAngleX <= minArmRotation + 1) { ChangeTarget(false, true); }
            else { ChangeTarget(true, false); }



            currentHandAngleX = Mathf.Clamp(currentHandAngleX, minHandRotation, maxHandRotation);
            handPivot.transform.localRotation = Quaternion.Euler(currentHandAngleX, 0, 0);
        }
    }

    private void ChangeTarget(bool isAimingAtSelf, bool canShoot)
    {
        player.ChangeTarget(isAimingAtSelf, canShoot);
    }
}
