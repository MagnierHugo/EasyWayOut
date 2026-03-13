using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Numerics;

public class RecordingCamera : MonoBehaviour {
    [SerializeField] private float ViewRange = 45.0f;
    [SerializeField] private float RotateSpeed = 0.5f;
    private UnityEngine.Vector3 RotationOffset = new UnityEngine.Vector3(0.0f, 0.0f, 0.0f);
    private float CurrentTime = 0.0f;

    void Start() {
        RotationOffset = new UnityEngine.Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
    void Update() {
        CurrentTime += Time.deltaTime;
        transform.rotation = UnityEngine.Quaternion.Euler(RotationOffset.x, (Mathf.Cos(CurrentTime * RotateSpeed) * ViewRange) + RotationOffset.y, RotationOffset.z);
    }
}