using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class MagManager {
    [SerializeField] private List<bool> slots = new List<bool>();
    private int currentIndex = 0;

    public void Init(int magCount) {
        slots = new List<bool>(magCount);
        currentIndex = Random.Range(0, slots.Count);
    }

    

    /// <summary>
    /// preserves mag order but change the relevant chamber
    /// </summary>
    public void Shuffle() {
        int shuffleForce = Random.Range(5, 11);
        currentIndex = (currentIndex + shuffleForce) % slots.Count;
    }


    public void AddBullet() {
        for (int idx = 0; idx < slots.Count; idx++) {
            if (!slots[idx]) {
                slots[idx] = true;
                return;
            }
        }
        Debug.LogError("Failed to add a bullet");
        return;
    }

    public bool NextBulletIsLive() => slots[currentIndex];

    public void ShootBullet() {
        slots[currentIndex] = false;
        currentIndex = (currentIndex + 1) % slots.Count;
    }
}
