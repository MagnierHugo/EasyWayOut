using UnityEngine;
using System.Collections;

public class OpponentMover : MonoBehaviour
{
    [Header("Settings")]
    public float dropDuration = 0.8f;
    public float fallBackDuration = 1.2f;


    public void DropIntoChair()
    {
        Vector3 startPos = new Vector3(0.65f, 5f, 0f);
        Quaternion startRot = Quaternion.Euler(0f, 180f, 0f);

        Vector3 endPos = new Vector3(0.65f, 1.4f, 0f);
        Quaternion endRot = Quaternion.Euler(0f, 180f, 0f);

        StopAllCoroutines();
        StartCoroutine(MoveRoutine(startPos, startRot, endPos, endRot, dropDuration, true));
    }

    public void FallBackward()
    {
        Vector3 startPos = new Vector3(0.65f, 1.4f, 0f);
        Quaternion startRot = Quaternion.Euler(0f, 180f, 0f);

        Vector3 endPos = new Vector3(0.65f, 1.5f, -2.5f);
        Quaternion endRot = Quaternion.Euler(90f, 180f, 0f);

        StopAllCoroutines();
        StartCoroutine(MoveRoutine(startPos, startRot, endPos, endRot, fallBackDuration, false));
    }

    private IEnumerator MoveRoutine(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, float duration, bool useEaseOut)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT;

            if (useEaseOut)
            {
                curveT = 1f - Mathf.Pow(1f - t, 3f);
            }
            else
            {
                curveT = t * t * (3f - 2f * t);
            }
            
            transform.position = Vector3.Lerp(startPos, endPos, curveT);
            transform.rotation = Quaternion.Slerp(startRot, endRot, curveT);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;
    }
}