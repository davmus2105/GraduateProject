using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float limitY = 40f;
    public float cameraMinDistance = 0.65f;
    public LayerMask obstacles;
    public LayerMask noPlayer;
    // -------- SENSETIVITY -----------
    // -----Temp------
        [SerializeField] [Range(10f, 100f)] private float _sensitivityX;
        [SerializeField] [Range(10f, 100f)] private float _sensitivityY;
    //private float _sensitivityX;
    //private float _sensitivityY;
    // --------------------------------
    // Var for distance of camera from player
    [SerializeField] [Range(1, 2.5f)] private float _cameraMaxDistance;
    // Var for GameController with all settings
    private GameObject _setupfolder;
    // Var with all settings
    // --- private GameSettings _gamesettings;
    // Var for object is looked by camera

    private Vector3 _localPosition;
    private LayerMask _camOrigin;
    private float _currYRot;
    private Vector3 _position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }


    private void Awake()
    {
        cam = Camera.main;
        _setupfolder = GameObject.Find("[SETUP]");
        //_gamesettings = _setupfolder.GetComponent<GameController>().gamesettings;
    }

    private void Start()
    {
        _sensitivityX = 100f;// --- _gamesettings.mouseSensitivityX;
        _sensitivityY = 100f;// --- _gamesettings.mouseSensitivityY;
        _localPosition = target.transform.InverseTransformPoint(_position);
        _cameraMaxDistance = Vector3.Distance(_position, target.position);
        _camOrigin = cam.cullingMask;

    }

    private void LateUpdate()
    {
        _position = target.TransformPoint(_localPosition);
        CameraRotation();
        ObstaclesReact();
        PlayerReact();

        _localPosition = target.transform.InverseTransformPoint(_position);
    }



    private void CameraRotation()
    {
        var mx = Input.GetAxis("Mouse X");
        var my = Input.GetAxis("Mouse Y");

        if (my != 0)
        {
            var tmp = Mathf.Clamp(_currYRot + my * _sensitivityY * Time.deltaTime, -limitY, limitY);
            if (tmp != _currYRot)
            {
                var rot = tmp - _currYRot;
                transform.RotateAround(target.position, transform.right, rot);
                _currYRot = tmp;
            }
        }
        if (mx > 0.1f || mx < 0.1f)
        {
            transform.RotateAround(target.position, Vector3.up,
                                   mx * _sensitivityX * Time.deltaTime);
        }       
        transform.LookAt(target);
    }

    private void ObstaclesReact()
    {
        var distance = Vector3.Distance(_position, target.position);
        RaycastHit hit;
        if (Physics.Raycast(target.position, 
                            transform.position - target.position, 
                            out hit, _cameraMaxDistance, obstacles))
        {
            _position = hit.point;
        }
        else if (distance < _cameraMaxDistance && !Physics.Raycast(_position, 
                                                                   -transform.forward, 
                                                                   .1f, obstacles))
        {
            _position -= transform.forward * .05f;
        }
    }

    private void PlayerReact()
    {
        var distance = Vector3.Distance(_position, target.position);
        if (distance < cameraMinDistance)
        {
            cam.cullingMask = noPlayer;
        }
        else
        {
            cam.cullingMask = _camOrigin;
        }
    }
}
