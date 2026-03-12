using System.Collections.Generic;
using UnityEngine;

public class MagManager : MonoBehaviour {
    private List<bool> Slots = new List<bool>();
    private int CurrentIndex = 0;

    public void InitMag(int MagCount) {
        Slots = new List<bool>(MagCount);
        CurrentIndex = Random.Range(0, Slots.Count - 1);
    }
        

    public void Shuffle() {
        int ShuffleForce = Random.Range(5, 11);
        CurrentIndex = (CurrentIndex + ShuffleForce) % Slots.Count;
    }


    public void AddBullet() {
        for (int idx = 0; idx < Slots.Count; idx++) {
            if (Slots[idx] == false) {
                Slots[idx] = true;
                return;
            }
        }
        return;
    }

    public bool GetBullet() {
        return Slots[CurrentIndex];
    }
}
