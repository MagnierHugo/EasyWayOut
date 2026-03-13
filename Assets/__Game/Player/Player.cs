using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour, IShootable
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isOpponent = false;

    [SerializeField] private Player otherPlayer = null;
    [SerializeField] private GameObject ShootSelfButton = null;
    [SerializeField] private GameObject ShootOpponentButton = null;
    [SerializeField] private GameObject SpecialButton = null;

    private Gun heldWeapon = null;
    private bool shotSelf = false;
    private bool weaponHasSpecial = false;

    // You would drag your UI Canvas holding the "Shoot Self" and "Shoot Opponent" buttons here
    // public GameObject actionUI; 

    public void MakeAChoice()
    {
        if (isOpponent) return;

        Debug.Log("Player's turn. Waiting for input...");

        // 1. Turn on the UI so the player can click a button
        // actionUI.SetActive(true); 
    }

    public void MakeAutoChoice()
    {
        if (!isOpponent) return;

        Debug.Log("Opponent is thinking...");

        // 50/50 chance to shoot self or player.
        float randomChoice = Random.value;

        // Little delay for more tension
        StartCoroutine(AIDecisionDelay(randomChoice));
    }

    private IEnumerator AIDecisionDelay(float choice)
    {
        yield return new WaitForSeconds(1.5f);

        ShootOpponent();

        //if (choice > 0.5f)
        //{
        //    ShootSelf();
        //}
        //else
        //{
        //    ShootOpponent();
        //}
    }

    private void Shoot(IShootable target)
    {
        heldWeapon.Shoot(target);

        if (isOpponent) return;
        SpecialButton.SetActive(false);
        ShootSelfButton.SetActive(false);
        ShootOpponentButton.SetActive(false);
    }

    public void ShootSelf()
    {
        // actionUI.SetActive(false); // Hide the buttons once a choice is made

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at themselves.");

        // PLAY ANIMATION HERE: Point gun at own head
        shotSelf = true;
        Shoot(this);
    }

    public void ShootOpponent()
    {
        // actionUI.SetActive(false); 

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at the enemy.");

        // PLAY ANIMATION HERE: Point gun at enemy
        Shoot(otherPlayer);
    }

    public void UseSpecial()
    {
        if (!weaponHasSpecial) return;

        (heldWeapon as IHaveSpecial).Special(this);

        if (isOpponent) return;
        SpecialButton.SetActive(false);
        ShootSelfButton.SetActive(false);
        ShootOpponentButton.SetActive(false);
    }

    public void GetShot()
    {
        if (isOpponent) { gameManager.OpponentDied(); }
        else { gameManager.PlayerDied(); }
    }

    public void EmptyShot()
    {
        if (!shotSelf)
            gameManager.ChangeWeaponSide();
        else
            gameManager.PlayTurn();

        shotSelf = false; 
    }

    public void ChangeTarget(bool isAimingAtSelf, bool canShoot)
    {
        if (isOpponent || !gameManager.playerHasGun) return;

        if (canShoot)
        {
            if (weaponHasSpecial) SpecialButton.SetActive(true);

            if (isAimingAtSelf) { ShootSelfButton.SetActive(true); }
            else { ShootOpponentButton.SetActive(true); }
            return;
        }

        SpecialButton.SetActive(false);
        ShootSelfButton.SetActive(false);
        ShootOpponentButton.SetActive(false);
    }

    public void EquipWeapon(Gun newWeapon)
    {
        heldWeapon = newWeapon;
        weaponHasSpecial = heldWeapon is IHaveSpecial;
    }
}