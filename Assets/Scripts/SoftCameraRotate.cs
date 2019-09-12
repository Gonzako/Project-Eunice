using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftCameraRotate : MonoBehaviour
{
    public Vector3 intensity;
    public float clamp;
    public Vector3 StartingPoint;
    private float counter = 0f;


    void Update()
    {
        Vector3 eulerRotation = new Vector3(Mathf.Sin(counter) * intensity.x ,Mathf.Cos(counter) * intensity.y ,Mathf.Sin(counter) * intensity.y);
        eulerRotation += StartingPoint;
        transform.rotation = Quaternion.Euler(-eulerRotation);
        counter += Time.deltaTime;
    }
}
