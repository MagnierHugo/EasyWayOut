using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour, IShootable
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isOpponent = false;

    [SerializeField] private Player otherPlayer = null;
    [SerializeField] private PlayerCamera PlayerCamera_ = null;
    [SerializeField] private GameObject ShootSelfButton = null;
    [SerializeField] private GameObject ShootOpponentButton = null;
    [SerializeField] private GameObject SpecialButton = null;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;


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

    private void Shoot(IShootable target) {
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
        PlayerCamera_.AddStress(2.0f);
    }

    public void ShootOpponent()
    {
        // actionUI.SetActive(false); 

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at the enemy.");

        // PLAY ANIMATION HERE: Point gun at enemy
        Shoot(otherPlayer);
        PlayerCamera_.AddStress(-2.0f);
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
            otherPlayer.animator.SetBool("DropGun", true);
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

    public void ChangeTarget(Target target)
    {
        if (isOpponent || !gameManager.playerHasGun) return;

        if (target == Target.None)
        {
            animator.SetBool("AimNone", true);
            SpecialButton.SetActive(false);
            ShootSelfButton.SetActive(false);
            ShootOpponentButton.SetActive(false);
            return;
        }

        if (target == Target.Self) animator.SetBool("AimSelf", true);
        else animator.SetBool("AimOp", true);
    }

    public void EquipWeapon(Gun newWeapon)
    {
        heldWeapon = newWeapon;
        weaponHasSpecial = heldWeapon is IHaveSpecial;
    }

    public void GrabWeapon() => heldWeapon.transform.SetParent(hand);
    public void DropWeapon()
    {
        heldWeapon.transform.SetParent(null);
        print(nameof(DropWeapon));
    }
    public void StartGrabAnimation() => animator.SetBool("GrabGun", true);

    public void OnGrabAnimationEnd()
    {
        animator.SetBool("GrabGun", false);
        gameManager.PlayTurn();
    }

    public void OnDropAnimationEnd()
    {
        animator.SetBool("DropGun", false);
        gameManager.ChangeWeaponSide();
    }

    public void OnAimSelfAnimationEnd()
    {
        animator.SetBool("AimSelf", false);
        ShootSelfButton.SetActive(true);
        if (weaponHasSpecial) SpecialButton.SetActive(true);
    }

    public void OnAimOpponentAnimationEnd()
    {
        animator.SetBool("AimOp", false);
        ShootOpponentButton.SetActive(true);
    }

    public void OnAimNoneAnimatedEnd()
    {
        animator.SetBool("AimNone", false);
    }
}