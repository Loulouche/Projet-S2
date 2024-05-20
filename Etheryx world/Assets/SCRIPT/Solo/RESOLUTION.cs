using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RESOLUTION : MonoBehaviour
{
    public TMP_Dropdown Resolution;

    public void SetResolution()
    {
        switch (Resolution.value)
        {
            case 1:
                Screen.SetResolution(640,360,true);
                break;
            case 0 : 
                Screen.SetResolution(1920,1080,true);
                break;
        }
    }

}
