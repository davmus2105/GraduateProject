using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HUD_Controller : MonoBehaviour
{
    const float STD_MESSAGE_TERM = 3f;
    GameObject hud;
    GameObject infomessage_panel;
    Text infomessage_text;
    GameObject inventory_panel;
    GameObject weapon;


    private static HUD_Controller _instance;
    public static HUD_Controller Instance => _instance;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        hud = GameObject.Find("[UI]").transform.Find("HUD").gameObject;
        infomessage_panel = hud.transform.Find("InfoMessagePanel").gameObject;
        infomessage_text = infomessage_panel.GetComponentInChildren<Text>();
        infomessage_panel.SetActive(false);
        inventory_panel = hud.transform.Find("InventoryPanel").gameObject;
        weapon = inventory_panel.transform.Find("Weapon").gameObject;
        weapon.SetActive(false);
    }
    public void ShowInfoMessage(string message, float sec = STD_MESSAGE_TERM)
    {            
        infomessage_text.text = message;        
        StartCoroutine("WaitForSec", sec);        
    }

    public void WeaponIsReady(bool isready)
    {
        weapon.SetActive(isready);
    }

    // Coroutines
    IEnumerator WaitForSec(float sec)
    {
        infomessage_panel.SetActive(true);
        yield return new WaitForSeconds(sec);
        infomessage_panel.SetActive(false);
    }
}

