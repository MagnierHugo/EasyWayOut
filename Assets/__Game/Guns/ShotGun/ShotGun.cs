using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static SelectShotgunRoundSystem;

[RequireComponent(typeof(AudioSource))]
public sealed class Shotgun : MonoBehaviour, IShoot
{
    private readonly Mag mag = new Mag();

    private ParticleSystem muzzleFlash;
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip pumpingAudio;
    [SerializeField] private AudioClip gunShotAudio;
    [SerializeField] private AudioClip gunClickAudio;

    public TransformData idleTransformData;
    public TransformData pointedAtOpponentTransformData;

    public Animator animator;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void LoadShells(int liveRoundCount)
    {
        mag.Init(8);
        for (int i = 0; i < liveRoundCount; i++)
            mag.AddBullet();

        mag.ShuffleRandom();
    }

    public void Shoot(IShootable target)
    {
        if (target == null)
            return;

        if (mag.GetBullet())
        {
            muzzleFlash.Play();
            audioSource.PlayOneShot(gunShotAudio);
            target.GetShot();
        }
        else
        {
            audioSource.PlayOneShot(gunClickAudio);
            target.EmptyShot();
        }

        animator.SetBool("shouldPickupGun", false);
    }

}