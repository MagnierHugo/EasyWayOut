using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mag {
    [SerializeField] private List<bool> slots = new List<bool>();
    private int currentIndex = 0;

    public void InitMag(int MagCount) {
        slots = new List<bool>(MagCount);
        currentIndex = Random.Range(0, slots.Count);
    }

    public void Shuffle() {
        int ShuffleForce = Random.Range(5, 11);
        currentIndex = (currentIndex + ShuffleForce) % slots.Count;
    }

    public void AddBullet() {
        for (int idx = 0; idx < slots.Count; idx++) {
            if (slots[idx] == false) {
                slots[idx] = true;
                return;
            }
        }
        return;
    }

    public bool GetBullet() {
        bool bullet = slots[currentIndex];
        slots[currentIndex] = false;
        currentIndex = (currentIndex + 1) % slots.Count;
        return bullet;
    }
}
