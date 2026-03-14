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
    public Animator animator;
    public PlayerAnimationEvents animationEvents;
    [SerializeField] private Transform hand;


    private Gun heldWeapon = null;
    private bool shotSelf = false;
    private bool weaponHasSpecial = false;
    private AIPersonality opponentPersonality = AIPersonality.Maniac;

    public bool dead = false;

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

    private void Shoot(IShootable target) {
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
        PlayerCamera_?.AddStress(2.0f);
    }

    public void ShootOpponent()
    {
        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at the enemy.");

        // PLAY ANIMATION HERE: Point gun at enemy
        Shoot(otherPlayer);
        PlayerCamera_?.AddStress(-2.0f);
    }

    public void UseSpecial()
    {
        if (!weaponHasSpecial)
            return;

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

        if (shotSelf)
        {
            animator.SetBool("DropGun", true);
            dead = true;
        }
        else
        {
            otherPlayer.animator.SetBool("DropGun", true);
            otherPlayer.dead = true;

        }
    }

    public void EmptyShot(bool again = true)
    {
        if (!shotSelf || !again)
        {
            print(nameof(EmptyShot));
            print(animator);
            print(animator.GetCurrentAnimatorStateInfo(0));
            otherPlayer.animator.SetBool("DropGun", true);
        }
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

    public void GrabWeapon() => heldWeapon.transform.SetParent(hand);
    public void DropWeapon() => heldWeapon.transform.SetParent(null);
    public void StartGrabAnimation() => animator.SetBool("GrabGun", true);

    public void OnGrabAnimationEnd()
    {
        animator.SetBool("GrabGun", false);
        gameManager.PlayTurn();
    }

    public void OnDropAnimationEnd()
    {
        animator.SetBool("DropGun", false);

        if (dead)
        {
            dead = false;
            return;
        }

        gameManager.ChangeWeaponSide();
    }

    public void OnAimSelfAnimationEnd()
    {
        animator.SetBool("AimSelf", false);
        ShootSelfButton.SetActive(true);
        if (weaponHasSpecial && !isOpponent) SpecialButton.SetActive(true);
    }

    public void OnAimOpponentAnimationEnd()
    {
        animator.SetBool("AimOp", false);
        if (!isOpponent) ShootOpponentButton.SetActive(true);
    }

    public void OnAimNoneAnimatedEnd()
    {
        animator.SetBool("AimNone", false);
    }
}