using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public sealed class SelectShotgunRoundSystem : MonoBehaviour
{
    private Ray ViewRay => camera.ScreenPointToRay(
        Mouse.current.position.ReadValue()
    );
    [SerializeField] private new Camera camera;
    private static LayerMask roundsLayer;
    private ShotgunRound hovered;

    private int roundLeftToSelect = 3;
    private int liveRoundCount = 1;
    private bool readyToSelect;

    [SerializeField] private ShotgunRound liveRoundPrefab;
    private ShotgunRound liveRoundInstance;
    private Vector3 liveRoundAnchorPoint;

    [SerializeField] private ShotgunRound blankRoundPrefab;
    private ShotgunRound blankRoundInstance;
    private Vector3 blankRoundAnchorPoint;


    [SerializeField] private float lerpToPositionDuration;
    [SerializeField] private float durationBetweenSelections;
    private WaitForSeconds awaitDurationBetweenSelections;
    [SerializeField] private float timeBeforeAnimationSelectsRound;
    private WaitForSeconds awaitAnimationSelectsRound;
    [SerializeField] private float durationAnimationHoversRound;
    private WaitForSeconds awaitDurationAnimationHoversRound;


    private bool demonstrationSequenceOver;
    [SerializeField] private float velocityUponDiscardCoefficient;

    [SerializeField] private Shotgun shotgun;

    private void Awake()
    {
        Transform temp = transform.GetChild(0);
        blankRoundInstance = Instantiate(blankRoundPrefab, temp);
        blankRoundInstance.transform.SetPositionAndRotation(transform.position, transform.rotation);
        blankRoundInstance.gameObject.SetActive(false);
        blankRoundAnchorPoint = temp.position;

        temp = transform.GetChild(1);
        liveRoundInstance = Instantiate(liveRoundPrefab, temp);
        liveRoundInstance.transform.SetPositionAndRotation(transform.position, transform.rotation);
        liveRoundInstance.gameObject.SetActive(false);
        liveRoundAnchorPoint = temp.position;

        roundsLayer = LayerMask.GetMask("ShotgunRounds");

        awaitDurationBetweenSelections = new WaitForSeconds(durationBetweenSelections);
        awaitAnimationSelectsRound = new WaitForSeconds(timeBeforeAnimationSelectsRound);
        awaitDurationAnimationHoversRound = new WaitForSeconds(durationAnimationHoversRound);

        StartRoundSelectionSequence();
    }

    private void StartRoundSelectionSequence()
    {
        if (hovered != null)
            ClearHovered();

        SpawnRounds();
        StartCoroutine(
            MoveRoundsIntoView(
                demonstrationSequenceOver ? 
                    () => readyToSelect = true :
                    () => { StartCoroutine(PlayRoundSelectionDemonstration()); }
            )
        );
    }

    private void SpawnRounds()
    {
        blankRoundInstance.transform.SetPositionAndRotation(transform.position, transform.rotation);
        liveRoundInstance.transform.SetPositionAndRotation(transform.position, transform.rotation);

        blankRoundInstance.gameObject.SetActive(true);
        liveRoundInstance.gameObject.SetActive(true);
    }
    private bool forceStopMovement;
    private IEnumerator MoveRoundsIntoView(Action callback)
    {
        Debug.Log("Started lerp");
        forceStopMovement = false;
        float elapsed = 0;
        yield return new WaitUntil(
            () =>
            {
                bool condition = forceStopMovement || elapsed > lerpToPositionDuration;
                elapsed += Time.time;

                float a = elapsed / lerpToPositionDuration;
                liveRoundInstance.transform.position = Vector3.Slerp(
                    liveRoundInstance.transform.position,
                    liveRoundAnchorPoint,
                    a
                );
                blankRoundInstance.transform.position = Vector3.Slerp(
                    blankRoundInstance.transform.position,
                    blankRoundAnchorPoint,
                    a
                );

                return condition;
            }
        );

        forceStopMovement = false;
        Debug.Log("Ended lerp");
        callback?.Invoke();
    }

    private IEnumerator PlayRoundSelectionDemonstration()
    {
        yield return awaitAnimationSelectsRound;

        liveRoundInstance.OnHoverEnter();

        yield return awaitDurationAnimationHoversRound;

        liveRoundInstance.OnHoverExit();
        StartCoroutine(StartRoundSelectedSequence(true));
    }
    private IEnumerator StartRoundSelectedSequence(bool selectedLiveOne)
    {
        forceStopMovement = true;
        Rigidbody relevantRigidBody = selectedLiveOne ? 
            blankRoundInstance.AddComponent<Rigidbody>():
            liveRoundInstance.AddComponent<Rigidbody>()
        ;

        Vector3 discardVelocity;
        do discardVelocity = UnityEngine.Random.insideUnitSphere;
        while (discardVelocity.y < .0f);

        relevantRigidBody.AddRelativeTorque(UnityEngine.Random.insideUnitSphere, ForceMode.VelocityChange);
        relevantRigidBody.AddForce(discardVelocity * velocityUponDiscardCoefficient, ForceMode.VelocityChange);

        yield return new WaitForSeconds(2f);

        ClearRounds(relevantRigidBody);

        yield return awaitDurationBetweenSelections;

        demonstrationSequenceOver = true;
        if (roundLeftToSelect > 0)
            StartRoundSelectionSequence();
        else
            shotgun.LoadShells(liveRoundCount);
    }

    private void ClearRounds(Rigidbody relevantRigidbody)
    {
        Debug.Log(nameof(ClearRounds));

        Destroy(relevantRigidbody);

        blankRoundInstance.gameObject.SetActive(false);
        liveRoundInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!demonstrationSequenceOver)
            return;

        if (!readyToSelect)
            return;

        if (roundLeftToSelect == 0)
            return;

        bool alreadyHoveringSth = hovered != null;
        if (Physics.Raycast(ViewRay, out var hit, 10, roundsLayer, QueryTriggerInteraction.Ignore))
        {
            GameObject hoveredGameObject = hit.collider.gameObject;
            if (alreadyHoveringSth)
                if (hoveredGameObject == hovered.gameObject)
                    goto Process;
                else
                    hovered.OnHoverExit();


            if (hoveredGameObject.TryGetComponent<ShotgunRound>(out hovered))
                hovered.OnHoverEnter();
        }
        else if (alreadyHoveringSth)
        {
            ClearHovered();
            return;
        }

    Process:
        if (hovered != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            liveRoundCount += hovered.IsLive ? 1 : 0;
            --roundLeftToSelect;
            StartCoroutine(
                StartRoundSelectedSequence(hovered.IsLive)
            );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearHovered()
    {
        hovered.OnHoverExit();
        hovered = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, .1f);
    }
}
