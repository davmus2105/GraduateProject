using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed, yPosMinus;
    public float xRotClamp = 45f, yRotClamp = 180f;
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
        yPosMinus = .5f;
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

        //if (pivot.rotation.eulerAngles.x > 45f && pivot.rotation.eulerAngles.y < 180f)
        //    pivot.rotation = Quaternion.Euler(45f, 0, 0);

        //if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.y < 315f)
        //    pivot.rotation = Quaternion.Euler(315f, 0, 0);

        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - yPosMinus, transform.position.z);
        }
        transform.LookAt(target);
    }
}
