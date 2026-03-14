using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public struct CameraShakeBehavior {
    public float SwayingForce;
    public float SwayingSpeed;
}

[System.Serializable]
public struct CameraShakeEntry {
    public string Name;
    public CameraShakeBehavior Behavior;
}

public class PlayerCamera : MonoBehaviour {
    [SerializeField] private string CurrentCameraShakeBehavior;
    
    // 1. Use a List so it shows up in the Inspector
    [SerializeField] private List<CameraShakeEntry> BehaviorList = new List<CameraShakeEntry>();

    // 2. This will be populated at runtime for fast lookups
    private Dictionary<string, CameraShakeBehavior> behaviorDict = new Dictionary<string, CameraShakeBehavior>();

    private Vector3 RotationOffset;

    [SerializeField] private float MentalStrenght = 100;

    void Awake() {
        MentalStrenght = Random.Range(100, 150);
        // 3. Convert the List to a Dictionary for easy access in Update
        foreach (var entry in BehaviorList) {
            if (!behaviorDict.ContainsKey(entry.Name)) {
                behaviorDict.Add(entry.Name, entry.Behavior);
            }
        }
    }

    void Start() {
        // Use localEulerAngles to avoid gimbal lock issues when starting
        RotationOffset = transform.localEulerAngles;
    }

    void Update() {
        if (behaviorDict.TryGetValue(CurrentCameraShakeBehavior, out CameraShakeBehavior settings)) {
            float time = Time.time * settings.SwayingSpeed;

            // Use larger offsets (like 100f) to ensure different axes sample different noise patterns
            // float x = (Mathf.PerlinNoise(time, 100f) - 0.5f) * 2.0f * settings.SwayingForce;
            // float y = (Mathf.PerlinNoise(200f, time) - 0.5f) * 2.0f * settings.SwayingForce;
            // float z = (Mathf.PerlinNoise(time, time) - 0.5f) * 2.0f * settings.SwayingForce;

            float x = (Mathf.PerlinNoise(time, 0.0f) - 0.5f) * 2.0f * settings.SwayingForce;
            float y = (Mathf.PerlinNoise(0.0f, time) - 0.5f) * 2.0f * settings.SwayingForce;
            float z = (Mathf.PerlinNoise(time, time) - 0.5f) * 2.0f * settings.SwayingForce;

            transform.localRotation = Quaternion.Euler(
                RotationOffset.x + x,
                RotationOffset.y + y,
                RotationOffset.z + z
            );
        }
    }

    public void AddStress(float amount) {
        MentalStrenght += amount;
        if (MentalStrenght < 0) {
            MentalStrenght = 10;
        }
        if (MentalStrenght < 66.0f) {
            CurrentCameraShakeBehavior = "Idle";
        } else if (MentalStrenght < 33.0f) {
            CurrentCameraShakeBehavior = "Panic";
        } else {
            CurrentCameraShakeBehavior = "Terror";
        }
    }
}