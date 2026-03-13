using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class Mag {
    [SerializeField] private List<bool> slots = new List<bool>();

    private int numberOfBullets = 1;
    private int numberOfChamberLeft = 0;
    private int currentIndex = 0;

    public void InitMag(int MagCount) {
        slots = new List<bool>();

        for (int i = 0; i < MagCount; i++)
        {
            slots.Add(false);
        }

        numberOfChamberLeft = MagCount;
        slots[0] = true;

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
                numberOfBullets++;
                numberOfChamberLeft++;
                return;
            }
        }
        return;
    }

    public bool GetBullet() {
        Debug.Log(currentIndex);
        bool bullet = slots[currentIndex];
        slots[currentIndex] = false;
        currentIndex = (currentIndex + 1) % slots.Count;
        return bullet;
    }

    public float GetNumberOfBullets() => numberOfBullets;
    public float GetNumberOfChamberLeft() => numberOfChamberLeft;
}
