using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, Initializable
{
    public float dayLength = 120f;
    [SerializeField][Range(0, 1)] private float currentTime;
    
    private float hour, min;
    private string s_hour, s_min; // For output

    // Texture changing
    public GameObject clouds, sky;
    public Transform sunMoon;
    [SerializeField] private float offsetX;
    Renderer skyrend, cloudsrend;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        clouds = GameObject.Find("[SCENE]").transform.Find("===== STATIC =====").Find("Sky").Find("Clouds").gameObject;
        sky = GameObject.Find("[SCENE]").transform.Find("===== STATIC =====").Find("Sky").Find("SkyDome").gameObject;
        sunMoon = GameObject.Find("[SCENE]").transform.Find("===== DYNAMIC =====").Find("SunMoon");
        skyrend = sky.GetComponent<Renderer>();
        cloudsrend = clouds.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeTick();
        CurrentTimeControll();
        TextureChange();
        SunMoonRot();
        

    }
    void CurrentTimeControll()
    {
        currentTime += Time.deltaTime / dayLength;
        if (currentTime >= 1 || currentTime < 0)
            currentTime = 0;
    }
    void TimeTick()
    {
        hour = 24 * currentTime;
        min = 60 * (hour - Mathf.Floor(hour));
        if (min < 10)
            s_min = "0" + (int)min;
        else
            s_min = ((int)min).ToString();
        if (hour < 10)
            s_hour = "0" + (int)hour;
        else
            s_hour = ((int)hour).ToString();
    }
    void TextureChange()
    {
        offsetX = currentTime;
        skyrend.material.mainTextureOffset = new Vector2(offsetX, 0);
        cloudsrend.material.mainTextureOffset = new Vector2(offsetX, 0);
        if (offsetX > 1)
        {
            offsetX = 0;
            skyrend.material.mainTextureOffset = new Vector2(offsetX, 0);
            cloudsrend.material.mainTextureOffset = new Vector2(offsetX, 0);
        }
    }
    void SunMoonRot()
    {
        sunMoon.rotation = Quaternion.Euler((currentTime * 360f), 0f, 0f);
    }
}