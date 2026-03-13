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
    private Ray ViewRay => camera.ScreenPointToRay(Mouse.current.position.ReadValue());
    [SerializeField] private new Camera camera;
    private static LayerMask roundsLayer;
    private ShotgunRound hovered;

    private int roundLeftToSelect = 4;
    private readonly bool[] roundsSelection = new bool[4];
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

    private async void Awake()
    {
        blankRoundInstance = Instantiate(blankRoundPrefab, transform.position, Quaternion.identity);
        blankRoundInstance.gameObject.SetActive(false);
        blankRoundAnchorPoint = transform.GetChild(0).position;

        liveRoundInstance = Instantiate(liveRoundPrefab, transform.position, Quaternion.identity);
        liveRoundInstance.gameObject.SetActive(false);
        liveRoundAnchorPoint = transform.GetChild(1).position;

        roundsLayer = LayerMask.GetMask("ShotgunRounds");

        await Task.Delay(1000);

        awaitDurationBetweenSelections = new WaitForSeconds(durationBetweenSelections);
        awaitAnimationSelectsRound = new WaitForSeconds(timeBeforeAnimationSelectsRound);
        awaitDurationAnimationHoversRound = new WaitForSeconds(durationAnimationHoversRound);

        StartRoundSelectionSequence();

    }
    private void StartRoundSelectionSequence()
    {
        readyToSelect = true;

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
        blankRoundInstance.gameObject.SetActive(true);
        liveRoundInstance.gameObject.SetActive(true);
    }
    private bool forceStopMovement;
    private IEnumerator MoveRoundsIntoView(Action callback)
    {
        float elapsed = 0;
        yield return new WaitWhile(
            () =>
            {
                bool condition = !forceStopMovement && elapsed < lerpToPositionDuration ;
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
        callback?.Invoke();
    }

    private IEnumerator PlayRoundSelectionDemonstration()
    {
        yield return awaitAnimationSelectsRound;

        liveRoundInstance.OnHoverEnter();

        yield return awaitDurationAnimationHoversRound;

        liveRoundInstance.OnHoverExit();
        StartCoroutine(
                StartRoundSelectedSequence(
                    roundsSelection[--roundLeftToSelect] = true
                )
        );

        demonstrationSequenceOver = true;

    }
    private IEnumerator StartRoundSelectedSequence(bool selectedLiveOne)
    {
        //yield return new WaitForFixedUpdate();
        forceStopMovement = true;
        Rigidbody relevantRigidBody = selectedLiveOne ? 
            blankRoundInstance.AddComponent<Rigidbody>():
            liveRoundInstance.AddComponent<Rigidbody>()
        ;

        Vector3 discardVelocity;
        do discardVelocity = UnityEngine.Random.insideUnitSphere;
        while (discardVelocity.y < .0f);

        //relevantRigidBody.angularVelocity = relevantRigidBody.linearVelocity = Vector3.zero;
        relevantRigidBody.AddRelativeTorque(UnityEngine.Random.insideUnitSphere, ForceMode.VelocityChange);
        relevantRigidBody.AddForce(discardVelocity * velocityUponDiscardCoefficient, ForceMode.VelocityChange);

        yield return new WaitForSeconds(2f);

        ClearRounds(relevantRigidBody);

        yield return awaitDurationBetweenSelections;

        if (roundLeftToSelect > 0)
            StartRoundSelectionSequence();
        else
            Shotgun.Instance.LoadShells();
    }

    private IEnumerator TrackRigidbodyData(Rigidbody rigidbody)
    {
        yield return new WaitUntil(
            () =>
            {
                bool val = rigidbody == null;
                if (val)
                    Debug.Log(rigidbody.linearVelocity);
                return val;
            }
        );
    }

    private void ClearRounds(Rigidbody relevantRigidbody)
    {
        Destroy(relevantRigidbody);
        blankRoundInstance.gameObject.SetActive(false);
        blankRoundInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        liveRoundInstance.gameObject.SetActive(false);
        liveRoundInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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
            StartCoroutine(
                StartRoundSelectedSequence(
                    roundsSelection[--roundLeftToSelect] = hovered.IsLive
                    )
                );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearHovered()
    {
        hovered.OnHoverExit();
        hovered = null;
    }
}
