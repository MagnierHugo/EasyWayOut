using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager = null;

    [Header("State")]
    [SerializeField] private int currentRound = 0;
    [SerializeField] private Player player;
    [SerializeField] private Player opponent;
    [SerializeField] private GameObject opponentGameObject;

    [Header("Guns Prefab")]
    [SerializeField] private GameObject revolverPrefab = null;
    [SerializeField] private GameObject nailgunPrefab = null;
    [SerializeField] private GameObject shotgunPrefab = null;
    [SerializeField] private GameObject doubleBarrelPrefab = null;
    [SerializeField] private GameObject burstPrefab = null;

    public bool playerHasGun = true;
    private List<int> weaponList = new List<int> { 0, 1, 2, 3 };
    private Vector3 spawnPos = new Vector3(0.7f, 2, 1.7f);

    public Gun currentWeapon;

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        currentRound = 0;

        GameObject spawnedObject = Instantiate(revolverPrefab, spawnPos, Quaternion.identity);
        currentWeapon = spawnedObject.GetComponent<Gun>();

        player.EquipWeapon(currentWeapon);
        opponent.EquipWeapon(currentWeapon);
        PlayTurn();
        StartCoroutine(SlideToFirst());
    }

    private System.Collections.IEnumerator SlideToFirst()
    {
        yield return new WaitForSeconds(3f);

        WeaponMover mover = currentWeapon.GetComponent<WeaponMover>();

        if (playerHasGun)
        {
            mover.SlideToPlayer();
        }
        else
        {
            mover.SlideToOpponent();
        }
    }

    private void StartNewRound()
    {
        currentRound++;
        if (currentRound == 1) // should be 5 here
        {
            JustOneLastGame();
            return;
        }

        lightingManager.ChangeRound(currentRound);

        opponent.UpdatePersonality(currentRound);

        SpawnRandomWeapon();

        playerHasGun = true;
        PlayTurn();
    }

    public void PlayTurn()
    {
        if (playerHasGun)
        {
            player.MakeAChoice();
        }
        else
        {
            opponent.MakeAutoChoice();
        }

        // The game now waits here. 
        // Once the choice animation is done, the animation event 
        // should call gameManager.ChangeWeaponSide() to continue the loop.
    }

    public void ChangeWeaponSide()
    {
        playerHasGun = !playerHasGun;

        WeaponMover mover = currentWeapon.GetComponent<WeaponMover>();

        if (playerHasGun)
        {
            mover.SlideToPlayer();
        }
        else
        {
            mover.SlideToOpponent();
        }

        // Start the next turn
        Invoke("PlayTurn", mover.moveDuration);
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
        
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        int randomIndex = Random.Range(0, weaponList.Count);

        int chosenWeaponID = weaponList[randomIndex];
        weaponList.RemoveAt(randomIndex);

        GameObject weaponToSpawn = null;

        switch (chosenWeaponID)
        {
            case 0:
                weaponToSpawn = doubleBarrelPrefab;
                break;
            case 1:
                weaponToSpawn = shotgunPrefab;
                break;
            case 2:
                weaponToSpawn = nailgunPrefab;
                break;
            case 3:
                weaponToSpawn = burstPrefab;
                break;
        }

        if (weaponToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(weaponToSpawn, spawnPos, Quaternion.identity);

            currentWeapon = spawnedObject.GetComponent<Gun>();

            player.EquipWeapon(currentWeapon);
            opponent.EquipWeapon(currentWeapon);
        }
    }

    private void JustOneLastGame()
    {
        opponentGameObject.SetActive(false);

        GameObject spawnedObject = Instantiate(revolverPrefab, spawnPos, Quaternion.identity);
        currentWeapon = spawnedObject.GetComponent<Gun>();
    }
}