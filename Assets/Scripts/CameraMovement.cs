using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles both the CameraMovement and it's rotation, may want to split this for more modularity
/// </summary>
public class CameraMovement : MonoBehaviour
{

    #region PublicFields
    public float panSpeed;
    public float movespeed;
    public float verticalSens;

    public float rotateAmount;
    public float rotateSpeed; 
    #endregion

    #region PrivateFields
    private Ray groundChecker = new Ray();
    private RaycastHit groundPoint;
    private Vector3 inputVector;
    private Vector3 currentPosition;
    private Quaternion defaultRotation;
    private Rigidbody rb;

    [SerializeField]
    private float minHeight, maxHeight;
    #endregion

    private void CheckMovementInput()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
        inputVector.y = Input.GetAxisRaw("Mouse ScrollWheel") * verticalSens;
    }

    private void MoveCamera()
    {
        Vector3 worldVel = transform.TransformDirection(inputVector);
        rb.velocity = worldVel * movespeed;
        transform.Translate(new Vector3(0f, inputVector.y, 0f));
        currentPosition = transform.position;
        groundChecker.origin = currentPosition;
        Physics.Raycast(groundChecker, out groundPoint, float.PositiveInfinity, LayerMask.GetMask("Floor"));
        currentPosition.y = Mathf.Clamp(transform.position.y, minHeight + groundPoint.point.y, maxHeight + groundPoint.point.y);

        transform.position = currentPosition;
    }

    private void CameraRotation()
    {
        Vector3 origin = transform.rotation.eulerAngles;
        Vector3 destination = origin;

        if (Input.GetButton("Fire2"))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        if(destination != origin)
        {

            transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            transform.rotation = defaultRotation;
            
    }

    #region MonoBehaviour API
    private void Start()
    {
        defaultRotation = transform.rotation;
        groundChecker.origin = transform.position;
        groundChecker.direction = Vector3.down;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementInput();
        CameraRotation();
        MoveCamera();
    } 
    #endregion






}
