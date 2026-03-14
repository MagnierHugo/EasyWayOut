using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static SelectShotgunRoundSystem;

[RequireComponent(typeof(AudioSource))]
public sealed class Shotgun : Gun, IShoot
{
    public static Shotgun Instance { get; private set; }
    private ParticleSystem muzzleFlash;
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip pumpingAudio;
    [SerializeField] private AudioClip gunShotAudio;
    [SerializeField] private AudioClip gunClickAudio;

    public Animator animator;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        muzzleFlash = transform.GetChild(0).GetComponent<ParticleSystem>();

        GameObject temp = Camera.main.transform.GetChild(0).gameObject;
        temp.SetActive(true);
        temp.GetComponent<SelectShotgunRoundSystem>().shotgun = this;
    }

    public void LoadShells(int liveRoundCount)
    {
        mag.Init(8);
        for (int i = 0; i < liveRoundCount; i++)
            mag.AddBullet();

        mag.ShuffleRandom();
    }

    public override void Shoot(IShootable target)
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

        //animator.SetBool("shouldPickupGun", false);
    }

}