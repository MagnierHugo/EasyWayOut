using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RecordingCamera : MonoBehaviour {
    [SerializeField] private float ViewRange = 45.0f;
    [SerializeField] private float RotateSpeed = 0.5f;
    [SerializeField] private float YOffset = 0.0f;
    private float CurrentTime = 0.0f;

    void Update() {
        CurrentTime += Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, (Mathf.Cos(CurrentTime * RotateSpeed) * ViewRange) + YOffset, 0f);
    }
}