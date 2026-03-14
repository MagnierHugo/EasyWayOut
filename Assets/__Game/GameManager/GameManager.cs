using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    [SerializeField] LightingManager lightingManager = null;

    [Header("State")]
    [SerializeField] private int currentRound = 0;
    [SerializeField] private Player player;
    [SerializeField] private Player opponent;

    [Header("End")]
    [SerializeField] private GameObject gameAssets = null;
    [SerializeField] private GameObject endAssets = null;
    [SerializeField] private Revolver endRevolver = null;
    [SerializeField] private Canvas endCanvas = null;

    [Header("Guns Prefab")]
    [SerializeField] private GameObject revolverPrefab = null;
    [SerializeField] private GameObject nailgunPrefab = null;
    [SerializeField] private GameObject shotgunPrefab = null;
    [SerializeField] private GameObject doubleBarrelPrefab = null;
    [SerializeField] private GameObject burstPrefab = null;

    public bool playerHasGun = true;
    private readonly List<int> weaponList = new List<int> { 0, 1, 2, 3 };
    private Vector3 spawnPos = new Vector3(0.7f, 2, 1.7f);

    public Gun currentWeapon;

    private void Start() => InitGame();

    private void InitGame()
    {
        currentRound = 4;

        GameObject spawnedObject = Instantiate(burstPrefab, spawnPos, Quaternion.identity);
        currentWeapon = spawnedObject.GetComponent<Gun>();

        player.EquipWeapon(currentWeapon);
        opponent.EquipWeapon(currentWeapon);
        PlayTurn();
        StartCoroutine(SlideToFirst());
    }

    private IEnumerator SlideToFirst()
    {
        yield return new WaitForSeconds(1.5f);

        WeaponMover mover = currentWeapon.GetComponent<WeaponMover>();
        mover.manager = this;

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
        if (currentRound >= 5)
        {
            if (currentWeapon != null) Destroy(currentWeapon.gameObject);

            StartCoroutine(JustOneLastGame());
            return;
        }

        lightingManager.ChangeRound(currentRound);

        opponent.UpdatePersonality(currentRound);

        SpawnRandomWeapon();

        playerHasGun = true;
        StartCoroutine(SlideToFirst());
        //player.animator.SetBool("GrabGun", true);
        //PlayTurn();
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
        //Invoke("PlayTurn", mover.moveDuration);
    }

    public void CurrentPlayerGrabWeapon()
    {
        if (playerHasGun)
        {
            player.StartGrabAnimation();
        }
        else
        {
            opponent.StartGrabAnimation();
        }
    }

    public void OpponentDied()
    {
        StartCoroutine(PlayAnimations());
    }
    
    private IEnumerator PlayAnimations()
    {
        opponent.GetComponent<OpponentMover>().FallBackward();
        yield return new WaitForSeconds(4f);
        
        opponent.GetComponent<OpponentMover>().DropIntoChair();
        yield return new WaitForSeconds(2f);

        StartNewRound();
    }

    public void PlayerDied()
    {
        GameOver();
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

        switch (3)
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
            currentWeapon.GetComponent<WeaponMover>().manager = this;

            player.EquipWeapon(currentWeapon);
            opponent.EquipWeapon(currentWeapon);
        }
    }

    private IEnumerator JustOneLastGame()
    {
        gameAssets.SetActive(false);
        endAssets.SetActive(true);
        yield return new WaitForSeconds(1f);

        endRevolver.Shoot(player);
        yield return new WaitForSeconds(0.5f);

        GameOver();
    }

    private void GameOver()
    {
        endCanvas.enabled = true;
        StartCoroutine(CloseWithDelay());
    }

    private IEnumerator CloseWithDelay()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}