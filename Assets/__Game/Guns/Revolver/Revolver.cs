using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : Gun, IHaveSpecial
{
    private Mag mag;

    [Header("Components")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip revolverCockingAudio;
    [SerializeField] private AudioClip revolverFireAudio;
    [SerializeField] private AudioClip revolverEmptyAudio;

    private void Start()
    {
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
                audioSource.clip = revolverEmptyAudio;
                audioSource.Play();
            }

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
