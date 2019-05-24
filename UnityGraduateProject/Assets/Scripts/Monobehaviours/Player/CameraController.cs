using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot;
    private void Start()
    {
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        float inputX = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, inputX, 0);
        float inputY = Input.GetAxis("Mouse Y") * rotateSpeed;
        target.Rotate(inputY, 0, 0);
    }
}
