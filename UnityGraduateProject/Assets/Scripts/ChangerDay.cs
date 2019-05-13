using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerDay : MonoBehaviour
{
    public float ScrollX = 0.01f;
    public float ScrollY = 0f;
    //public GameObject Sun;
    //public GameObject Moon;
    //public float speed = 10f;

    // Update is called once per frame
     void Update()
    {
        float OffsetX = Time.time * ScrollX; // 0.5 > 0.01  time.delta = 0.02 * 0.5
        float OffsetY = Time.time * ScrollY;
        float counter = OffsetX;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX,OffsetY);
        if (OffsetX > 1.1f )
        {
            OffsetX = 0.2f;
        }
        /*Sun.transform.RotateAround(Vector3(0, 0, 0), Vector3.right, 10f * Time.deltaTime);
        Moon.transform.RotateAround(Vector3.zero, Vector3.right, 10f * Time.deltaTime);*/
        //Sun.transform.Rotate(-200, speed * Time.deltaTime, 0);

        /*if (counter > 0.2)
        {
            ScrollX = 0.01f;
            Debug.Log("Zero counter");
        }*/
    }
    
}
