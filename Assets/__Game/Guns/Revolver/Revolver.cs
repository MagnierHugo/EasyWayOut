using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : Gun, IHaveSpecial
{
    private Mag mag = new Mag();

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
        mag.InitMag(6);
    }

    public override void Shoot(IShootable target)
    {
        animator.SetTrigger("Cock Hammer");
        audioSource.clip = revolverCockingAudio;
        audioSource.Play();
        var cockBehaviour = animator.GetBehaviour<Cock_Behaviour>();

        // Would have used lambda but it can't ref itself
        void OnCockingAnimationEnd()
        {
            cockBehaviour.onCockAnimationEnd -= OnCockingAnimationEnd;
            bool bullet = mag.GetBullet();
            if (!bullet)
            {
                Debug.Log("Click.");
                target.EmptyShot();
                audioSource.clip = revolverEmptyAudio;
                audioSource.Play();
            }

            Debug.Log("BANG!");
            target.GetShot();
            muzzleFlash.Play();
            audioSource.clip = revolverFireAudio;
            audioSource.Play();

        }

        cockBehaviour.onCockAnimationEnd += OnCockingAnimationEnd;
    }

    public void Special(IShootable target)
    {
        mag.AddBullet();
        mag.Shuffle();
        Shoot(target);
    }
}
