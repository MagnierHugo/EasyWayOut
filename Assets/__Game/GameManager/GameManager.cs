using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager = null;

    public int currentRound = 0;
    public Player player;
    public Player opponent;

    private bool playerHasGun = true;
    private List<int> weaponList = new List<int> { 0, 1, 2, 3 };

    private void Start()
    {
        StartNewRound();
    }

    private void StartNewRound()
    {
        currentRound++;
        lightingManager.ChangeRound(currentRound);

        SpawnRandomWeapon();

        // Reset the gun to the player for the start of the round
        playerHasGun = true;
        PlayTurn();
    }

    private void PlayTurn()
    {
        if (playerHasGun)
        {
            // player.MakeAChoice();
        }
        else
        {
            // opponent.MakeAutoChoice();
        }

        // The game now waits here. 
        // Once the choice animation is done, the animation event 
        // should call gameManager.ChangeWeaponSide() to continue the loop.
    }

    public void ChangeWeaponSide()
    {
        // Flip the boolean
        playerHasGun = !playerHasGun;

        if (playerHasGun)
        {
            // Play Sending weapon to player animation
        }
        else
        {
            // Play Sending weapon to op animation
        }

        // Start the next turn
        PlayTurn();
    }

    public void OpponentDied()
    {
        // Play Change opponent animation
        // Once the new opponent sits down, start the next round
        StartNewRound();
    }

    public void PlayerDied()
    {
        // Game Over Screen
        Debug.Log("Player died. Game Over.");
    }

    private void SpawnRandomWeapon()
    {
        if (weaponList.Count == 0)
        {
            Debug.LogWarning("No more weapons in the list!");
            return;
        }

        int randomIndex = Random.Range(0, weaponList.Count);

        int chosenWeaponID = weaponList[randomIndex];
        weaponList.RemoveAt(randomIndex);

        switch (chosenWeaponID)
        {
            case 0:
                // Choose Double-Barrel
                break;
            case 1:
                // Choose Shotgun
                break;
            case 2:
                // Choose Nailgun
                break;
            case 3:
                // Choose Burst
                break;
        }
    }
}