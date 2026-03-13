using UnityEngine;
using System.Collections;

public class WeaponMover : MonoBehaviour
{
    [Header("Settings")]
    public float moveDuration = 1.0f;

    // Player side
    private Vector3 playerPos = new Vector3(0.6f, 2.07f, 2.7f);
    private Quaternion playerRot = Quaternion.Euler(0, -90, 90);
    
    // Opponent side
    private Vector3 opponentPos = new Vector3(0.8f, 2.07f, 0.6f);
    private Quaternion opponentRot = Quaternion.Euler(0, 90, 90);

    public void SlideToOpponent()
    {
        StopAllCoroutines();
        StartCoroutine(MoveWeapon(playerPos, playerRot, opponentPos, opponentRot, moveDuration));
    }

    public void SlideToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(MoveWeapon(opponentPos, opponentRot, playerPos, playerRot, moveDuration));
    }

    private IEnumerator MoveWeapon(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            // so the gun is slow then fast then slow again
            float smoothT = t * t * (3f - 2f * t);

            transform.SetPositionAndRotation(
                Vector3.Lerp(startPos, endPos, smoothT),
                Quaternion.Slerp(startRot, endRot, smoothT)
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.SetPositionAndRotation(endPos, endRot);
    }
}