using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// This class handles the how the player will click/interact with the world
/// 
/// We want this class to:
///     Differentiate between click and hold (left and right click/ gamepad trigger) 
///     Call events depending on what it clicked    
/// 
/// </summary>
public class ClickSorter : MonoBehaviour
{

    public GameObject selectedObject;
    public float timeDifForClick = 0.2f;


    #region Events
    public static event Action OnAnySelect = delegate { };
    public static event Action<RaycastHit> OnAction = delegate { };
    #endregion

    #region PrivateFields
    private ISelectable objectBeingSelected;
    private bool timerStarted;
    private float currentTime;
    private Camera currentCam; 
    #endregion


    /// <summary>
    /// This fuction currently only differentiates between rightclick and hold
    /// Raises onAction if doing only right click
    /// </summary>
    private void CheckForAction()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            timerStarted = true;
            currentTime = timeDifForClick;
        }

        if (Input.GetButtonUp("Fire2") && currentTime > 0)
        {
            int layerMask = ~LayerMask.GetMask("Drone");
            Physics.Raycast(currentCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.PositiveInfinity, layerMask); //HACK or feels like one
            timerStarted = false;
            OnAction.Invoke(hit);
            //Code to do Stuff here
        }

        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                timerStarted = false;
            }
        }
    }

    /// <summary>
    /// Creates raycast if clicked and selects hit
    /// </summary>
    private void CheckForSelection()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            Ray cameraRay = currentCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hit, 100, ~0))
            {
                objectBeingSelected = hit.collider.gameObject.GetComponent<ISelectable>();
                if (objectBeingSelected != null)
                {
                    selectedObject = hit.collider.gameObject;
                    objectBeingSelected.IsSelected = true;
                    OnAnySelect.Invoke();
                    Debug.Log("Object selected");
                    if (objectBeingSelected == null) Debug.LogError("Selectable object without selectable component");


                }
                else
                {
                    if (objectBeingSelected != null) objectBeingSelected.IsSelected = false;
                    selectedObject = null;
                    objectBeingSelected = null;
                    Debug.Log("Deselected");
                }

            }

        }


    }

    #region MonoBehaviourAPI
    private void Awake()
    {

        currentCam = Camera.main;
    }

    private void Update()
    {
        CheckForAction();
        CheckForSelection();
    } 
    #endregion
}
