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
    private AIPersonality opponentPersonality = AIPersonality.Maniac;

    public void MakeAChoice()
    {
        if (isOpponent) return;

        Debug.Log("Player's turn. Waiting for input...");
    }

    public void MakeAutoChoice()
    {
        if (!isOpponent) return;

        Debug.Log("Opponent is thinking...");

        StartCoroutine(AIDecisionDelay());
    }

    private IEnumerator AIDecisionDelay()
    {
        yield return new WaitForSeconds(1.5f);

        AIExecutioner.ExecuteAI(opponentPersonality, this, weaponHasSpecial, heldWeapon);
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

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at themselves.");

        // PLAY ANIMATION HERE: Point gun at own head
        shotSelf = true;
        Shoot(this);
    }

    public void ShootOpponent()
    {
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

    public void UpdatePersonality(int roundNumber)
    {
        switch(roundNumber)
        {
            case 0:
                opponentPersonality = AIPersonality.Maniac;
                break;

            case 1:
                opponentPersonality = AIPersonality.Thug;
                break;

            case 2:
                opponentPersonality = AIPersonality.Coward;
                break;

            case 3:
                opponentPersonality = AIPersonality.Gambler;
                break;

            case 4:
                opponentPersonality = AIPersonality.Calculator;
                break;
        }
    }
}