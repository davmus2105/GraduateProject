using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera_1 : MonoBehaviour
{
    [SerializeField] float offsetZ, offsetY;
    [SerializeField] float smooth;
    [SerializeField] Transform target;
    Camera cam;
    [SerializeField] Vector3 camPosDestination, offsetVector;

    [SerializeField] LayerMask layers;
    Ray ray;
    RaycastHit hit;


    void Start()
    {
        cam = Camera.main;
        target = transform.parent.Find("Model");
        offsetVector = new Vector3(0f, offsetY, -offsetZ);
    }

    void LateUpdate()
    {
        CameraFollowing();
    }
    void CameraFollowing()
    {      
        camPosDestination = target.position + offsetVector;
        transform.position = Vector3.Lerp(transform.position, camPosDestination, smooth * Time.deltaTime);
        transform.LookAt(target.position);
    }

}
