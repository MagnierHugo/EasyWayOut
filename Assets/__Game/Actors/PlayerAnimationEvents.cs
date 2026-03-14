using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    // Mid animations Events
    public UnityEvent OnGunGrabbed;
    public UnityEvent OnGunDropped;

    // Animation ends Events
    public UnityEvent OnGrabAnimationEnd;
    public UnityEvent OnDropAnimationEnd;
    public UnityEvent OnAimSelfAnimationEnd;
    public UnityEvent OnAimOpponentAnimationEnd;
    public UnityEvent OnAimNoneAnimationEnd;

    public void Invoke_OnGunGrabbed()
    {
        OnGunGrabbed?.Invoke();
        print(nameof(Invoke_OnGunGrabbed));
    }

    public void Invoke_OnGunDropped()
    {
        OnGunDropped?.Invoke();
        print(nameof(Invoke_OnGunDropped));
    }

    public void Invoke_OnDropAnimationEnd()
    {
        OnDropAnimationEnd?.Invoke();
        print(nameof(Invoke_OnDropAnimationEnd));
    }

    public void Invoke_OnGrabAnimationEnd()
    {
        OnGrabAnimationEnd?.Invoke();
        print(nameof(Invoke_OnGrabAnimationEnd));
    }

    public void Invoke_OnAimSelfAnimationEnd()
    {
        OnAimSelfAnimationEnd?.Invoke();
        print(nameof(Invoke_OnAimSelfAnimationEnd));
    }

    public void Invoke_OnAimOpponentAnimationEnd()
    {
        OnAimOpponentAnimationEnd?.Invoke();
        print(nameof(Invoke_OnAimOpponentAnimationEnd));
    }

    public void Invoke_OnAimNoneAnimationEnd()
    {
        OnAimNoneAnimationEnd?.Invoke();
        print(nameof(Invoke_OnAimNoneAnimationEnd));
    }
}
