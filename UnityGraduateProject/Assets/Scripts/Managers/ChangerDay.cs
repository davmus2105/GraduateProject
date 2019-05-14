using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerDay : MonoBehaviour
{
    public float Dayspeed = 0.01f;
    //public float ScrollY = 0f;
    public GameObject AllTimeDayPref;
    public GameObject Sun;
    public GameObject Moon;
    public GameObject clouds;
    public GameObject sky;
    Renderer skyrend,cloudsrend;
    private float counter = 1f;
    public float OffsetX;




    // Update is called once per frame
    private void Start()
    {
        skyrend = sky.GetComponent<Renderer>();//skyrend = GetComponent<Renderer>();
        cloudsrend = clouds.GetComponent<Renderer>();
        
    }
    void Update()
    {
       OffsetX = Time.time * Dayspeed; // 0.5 > 0.01  time.delta = 0.02 * 0.5
        skyrend.material.mainTextureOffset = new Vector2(OffsetX, 0); //cloudsrend.material.mainTextureOffset = new Vector2(OffsetX, 0);
        cloudsrend.material.mainTextureOffset = new Vector2(OffsetX, 0);
       
        
        //Sun.transform.Rotate(-200, speed * Time.deltaTime, 0);
        Sun.transform.RotateAround(Vector3.zero, Vector3.right, Dayspeed * 400 * Time.deltaTime);
        Moon.transform.RotateAround(Vector3.zero, Vector3.right, Dayspeed * 400 * Time.deltaTime);
        //sun_moon.rotation = Quaternion.Euler(sun_moon.rotation.x -speed,0,0);

        
          if (counter < OffsetX)
        {
            Debug.Log(Zero(OffsetX));// 51,00039
            OffsetX = Zero(OffsetX);
            skyrend.material.mainTextureOffset = new Vector2(OffsetX, 0);
            cloudsrend.material.mainTextureOffset = new Vector2(OffsetX, 0);
            Debug.Log("Offset is!!! Some changing");
        }
        /*else
        {
            OffsetX = Time.deltaTime * Dayspeed;
        }*/
    }
    public float Zero(float Value)
    {
        float a = 1f;
        
        return a;
    }


}
