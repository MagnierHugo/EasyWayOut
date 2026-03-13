using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagManager {
    [SerializeField] private List<bool> Slots = new List<bool>();
    private int CurrentIndex = 0;

    public void InitMag(int MagCount) {
        Slots = new List<bool>(MagCount);

        for (int i = 0; i < MagCount; i++)
        {
            Slots.Add(false);
        }

        Slots[0] = true;

        CurrentIndex = Random.Range(0, Slots.Count);
    }
        

    public void Shuffle() {
        int ShuffleForce = Random.Range(5, 11);
        CurrentIndex = (CurrentIndex + ShuffleForce) % Slots.Count;
    }


    public void AddBullet() {
        for (int idx = 0; idx < Slots.Count; idx++)
        {
            if (Slots[idx] == false) {
                Slots[idx] = true;
                return;
            }
        }
        return;
    }

    public bool GetBullet() {
        Debug.Log(CurrentIndex);
        return Slots[CurrentIndex];
    }
    
    public void ShootBullet() {
        Slots[CurrentIndex] = false;
        CurrentIndex = (CurrentIndex + 1) % Slots.Count;
    }
}
