using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public sealed class ShotgunAnimationEvent : MonoBehaviour
{
    [SerializeField] private Transform shotgunRoot;
    [SerializeField] private Transform handSocket;
    [SerializeField] private Transform enemy;



    [SerializeField] private Animator animator;
    private void Awake()
    {
        //animator.SetBool("shouldPickupGun", true);
        
        Shotgun.Instance.animator = animator;
    }
    public static event Action OnGrabShotgun = () => print("Grabbed here");

    public void InvokeOnGrabShotgun()
    {
        OnGrabShotgun?.Invoke();
        //shotgunRoot.SetParent(handSocket);
        //shotgunRoot.SetLocalPositionAndRotation(
        //    //new Vector3(0.0127f, -0.0201f, 0.0141f),
        //    Vector3.zero,
        //    Quaternion.Euler(58.394f, 19.772f, 77.686f)
        //);
        StartCoroutine(FadeEffect.Fade(.5f, 1, .3f));
    }

    public void Tick()
    {
        // shotgunRoot.localPosition = Vector3.zero;
        // shotgunRoot.LookAt(enemy);
        // shotgunRoot.Rotate(0f, 180f, 0f);
    }

    public void OnAnimationEnd()
    {
        shotgunRoot.GetComponent<Shotgun>().pointedAtOpponentTransformData.Apply(shotgunRoot);
    }

}
