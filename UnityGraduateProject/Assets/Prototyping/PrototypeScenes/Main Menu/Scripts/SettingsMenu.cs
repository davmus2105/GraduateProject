using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public Dropdown ResolutionDropdown;
    public Dropdown GraphicsDropdown;
    Resolution[] res;
    void Start()
    {
        GraphicsDropdown.ClearOptions(); 
		GraphicsDropdown.AddOptions(QualitySettings.names.ToList()); 
		GraphicsDropdown.value = QualitySettings.GetQualityLevel();


        Resolution [] resolution = Screen.resolutions; 
        res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for(int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].ToString();
        } 
        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(strRes.ToList());
		ResolutionDropdown.value = res.Length - 1;

        Screen.SetResolution(res[res.Length-1].width, res[res.Length - 1].height, true);

       


    }

    public void SetRes ()
    {
       Screen.SetResolution(res[ResolutionDropdown.value].width, res[ResolutionDropdown.value].height,true);

    }
    public void SetFullScreen( bool isFull)
	{
		Screen.fullScreen = isFull;
	}
    public void SetQuality()
	{
	QualitySettings.SetQualityLevel(GraphicsDropdown.value);
		
	}
}
