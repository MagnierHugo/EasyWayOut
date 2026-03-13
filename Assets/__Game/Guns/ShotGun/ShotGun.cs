using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class Shotgun : MonoBehaviour, IShoot
{
    private readonly Mag mag = new Mag();

    private ParticleSystem muzzleFlash;
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip pumpingAudio;
    [SerializeField] private AudioClip gunShotAudio;
    [SerializeField] private AudioClip gunClickAudio;

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

        StartCoroutine(StartShootingSequence());
    }

    private IEnumerator StartShootingSequence()
    {
        yield return null;
    }

}