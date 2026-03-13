using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : Gun, IHaveSpecial
{
    private readonly Mag mag = new Mag();

    [Header("Components")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip revolverCockingAudio;
    [SerializeField] private AudioClip revolverFireAudio;
    [SerializeField] private AudioClip revolverEmptyAudio;

    private void Awake()
    {
        Debug.Log("Revolver");
        mag.Init(6);
    }

    public override void Shoot(IShootable target)
    {
        print(animator);
        print(nameof(Shoot));

        animator.SetBool("CockGun", true);
        audioSource.clip = revolverCockingAudio;
        audioSource.Play();

        var behaviours = animator.GetBehaviours<OnEndCallbackBehaviour>();
        OnEndCallbackBehaviour cockBehaviour = behaviours.Where(b => b.StateName == "Cock hammer").Single();
        OnEndCallbackBehaviour fireBehaviour = behaviours.Where(b => b.StateName == "Fire").Single();

        bool bullet = false;

        void OnFireAnimationEnd()
        {
            animator.SetBool("Fire", false);
            fireBehaviour.onAnimationEnd -= OnFireAnimationEnd;

            if (bullet) target.GetShot();
            else target.EmptyShot();
        }

        // Would have used lambda but it can't ref itself
        void OnCockingAnimationEnd()
        {
            animator.SetBool("CockGun", false);
            animator.SetBool("Fire", true);

            cockBehaviour.onAnimationEnd -= OnCockingAnimationEnd;
            fireBehaviour.onAnimationEnd += OnFireAnimationEnd;

            bullet = mag.GetBullet();
            if (bullet)
            {
                Debug.Log("BANG!");
                muzzleFlash.Play();
                audioSource.clip = revolverFireAudio;
                audioSource.Play();
            }
            else
            {
                Debug.Log("Click.");
                audioSource.clip = revolverEmptyAudio;
                audioSource.Play();
            }
        }

        cockBehaviour.onAnimationEnd += OnCockingAnimationEnd;
    }

    public void Special(IShootable target)
    {
        mag.AddBullet();
        mag.ShuffleShift();
        Shoot(target);
    }

    [ContextMenu("HELP")]
    public void Help()
    {
        var animationState = animator.GetAnimatorTransitionInfo(0);
    }
}
