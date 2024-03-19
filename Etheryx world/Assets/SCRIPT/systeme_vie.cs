using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class systeme_vie : MonoBehaviour
{
    public Sprite emptyPoint, fullPoint;
    public Image vie1, vie2, vie3, vie4, vie5;
    
    void Start()
    {
        
    }

    void Update()
    {
        switch (perso_principal.instance.vie)
        {
            case 0 :
                vie1.sprite = emptyPoint;
                vie2.sprite = emptyPoint;
                vie3.sprite = emptyPoint;
                vie4.sprite = emptyPoint;
                vie5.sprite = emptyPoint;
                break;
            case 1 :
                vie1.sprite = fullPoint;
                vie2.sprite = emptyPoint;
                vie3.sprite = emptyPoint;
                vie4.sprite = emptyPoint;
                vie5.sprite = emptyPoint;
                break;
            case 2 :
                vie1.sprite = fullPoint;
                vie2.sprite = fullPoint;
                vie3.sprite = emptyPoint;
                vie4.sprite = emptyPoint;
                vie5.sprite = emptyPoint;
                break;
            case 3 :
                vie1.sprite = fullPoint;
                vie2.sprite = fullPoint;
                vie3.sprite = fullPoint;
                vie4.sprite = emptyPoint;
                vie5.sprite = emptyPoint;
                break;
            case 4 :
                vie1.sprite = fullPoint;
                vie2.sprite = fullPoint;
                vie3.sprite = fullPoint;
                vie4.sprite = fullPoint;
                vie5.sprite = emptyPoint;
                break;
            case 5 :
                vie1.sprite = fullPoint;
                vie2.sprite = fullPoint;
                vie3.sprite = fullPoint;
                vie4.sprite = fullPoint;
                vie5.sprite = fullPoint;
                break;
        }
    }
}
