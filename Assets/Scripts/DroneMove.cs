using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Selectable))]
public class DroneMove : MonoBehaviour
{
    internal MoveState MoveState { get => moveState; set => moveState = value; }
    public Vector3 Destination { get => destination; }

    public float DroneSpeed;

    public event Action OnMoveFinish = delegate { };

    private Selectable selectable;
    private Vector3 destination;
    private MoveState moveState;



    private void GetDesiredLocation(RaycastHit target)
    {
        destination = target.collider.tag == "Interactible" ? target.collider.gameObject.transform.position : target.point;

        destination.y = transform.position.y;
        MoveState = MoveState.Moving;
    }

    #region MonoBehaviour API
    private void Update()
    {
        switch (MoveState)
        {
            case MoveState.Waiting:
                break;
            case MoveState.Moving:
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * DroneSpeed);
                if (transform.position == destination)
                    MoveState = MoveState.Finished;
                break;
            case MoveState.Finished:
                OnMoveFinish.Invoke();
                MoveState = MoveState.Waiting;
                break;

        }

    }

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        selectable.OnThisAction += GetDesiredLocation;
        destination = transform.position;

    }

    private void OnDestroy()
    {
        selectable.OnThisAction -= GetDesiredLocation;
    } 
    #endregion

}
