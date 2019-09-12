using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selectable))]
public class PickupCode : MonoBehaviour
{
    //PickupTransform gets picked by property injection in engine if not, it'll get the 1st child
    public Transform pickupTransform;

    #region Private Fields
    private Selectable selectable;
    private PickupState state;
    private DroneMove move;
    private GameObject objHit;
    #endregion

    private enum PickupState
    {
        Empty, Full, WaitingForMove
    }


    private void OnActionCall(RaycastHit obj)
    {


        if (obj.collider.tag == "Interactible")//HACK Tag check
        {
            move.OnMoveFinish += Grab;
            if (state == PickupState.Full) Drop();
            objHit = obj.collider.gameObject;

        } else if (state == PickupState.Full)
        {
           
           move.OnMoveFinish += Drop;
        }

    }

    private void Grab()
    {
        if (objHit == null) return;//HACK null check
        objHit.transform.SetParent(pickupTransform);
        objHit.transform.position = pickupTransform.position;
        Debug.Log("Pickup");
        state = PickupState.Full;
        move.OnMoveFinish -= Grab;
    }

    private void Drop()
    {
        if (objHit == null) return;//HACK null check
        Physics.Raycast(objHit.transform.position, Vector3.down, out RaycastHit hit);
        objHit.transform.position = hit.point;
        objHit.transform.Translate(0f,objHit.GetComponent<Collider>().bounds.extents.y, 0f);
        objHit.transform.parent = null;
        objHit = null;
        Debug.Log("Drop");
        state = PickupState.Empty;
        move.OnMoveFinish -= Drop;
    }


    #region MonoBehaviour API
    private void Awake()
    {
        if (pickupTransform == null) pickupTransform = transform.GetChild(0);
        state = PickupState.Empty;
        selectable = GetComponent<Selectable>();
        selectable.OnThisAction += OnActionCall;
        move = GetComponent<DroneMove>();
    }


    private void OnDestroy()
    {
        selectable.OnThisAction -= OnActionCall;
    }
    #endregion


    
}