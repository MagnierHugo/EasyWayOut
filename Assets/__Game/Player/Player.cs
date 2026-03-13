using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour, IShootable
{
    [Header("References")]
    public GameManager gameManager;
    public bool isOpponent = false;

    [SerializeField] private GameObject ShootSelfButton = null;
    [SerializeField] private GameObject ShootOpponentButton = null;

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

        if (choice > 0.5f)
        {
            ShootSelf();
        }
        else
        {
            ShootOpponent();
        }
    }

    public void ShootSelf()
    {
        // actionUI.SetActive(false); // Hide the buttons once a choice is made

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at themselves.");

        // PLAY ANIMATION HERE: Point gun at own head

        ResolveShot(shotSelf: true);
    }

    public void ShootOpponent()
    {
        // actionUI.SetActive(false); 

        Debug.Log((isOpponent ? "Opponent" : "Player") + " points the gun at the enemy.");

        // PLAY ANIMATION HERE: Point gun at enemy

        ResolveShot(shotSelf: false);
    }

    // --- RESOLVING THE OUTCOME ---

    private void ResolveShot(bool shotSelf)
    {
        // THIS IS WHERE YOU ASK THE WEAPON IF IT FIRED.
        // For now, let's fake a standard 1-in-6 revolver probability for testing:
        bool gunFired = Random.Range(0, 6) == 0;

        if (gunFired)
        {
            Debug.Log("BANG!");

            // PLAY ANIMATION HERE: Gun firing, muzzle flash, character dying

            if (shotSelf)
            {
                if (isOpponent) gameManager.OpponentDied();
                else gameManager.PlayerDied();
            }
            else
            {
                if (isOpponent) gameManager.PlayerDied();
                else gameManager.OpponentDied();
            }
        }
        else
        {
            Debug.Log("Click... Empty chamber.");

            // PLAY ANIMATION HERE: Gun clicking, character sighing in relief

            // The turn is over, tell the GameManager to pass the weapon
            gameManager.ChangeWeaponSide();
        }
    }

    public void GetShot()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeTarget(bool isAimingAtSelf, bool canShoot)
    {
        if (canShoot)
        {
            if (isAimingAtSelf) { ShootSelfButton.SetActive(true); }
            else { ShootOpponentButton.SetActive(true); }
        }
        else
        {
            ShootSelfButton.SetActive(false);
            ShootOpponentButton.SetActive(false);
        }
    }
}