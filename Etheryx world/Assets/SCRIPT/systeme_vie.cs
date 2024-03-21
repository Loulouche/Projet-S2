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
		float a = perso_principal.instance.vie;
        if (a >4)
		{
			vie1.sprite = fullPoint;
            vie2.sprite = fullPoint;
            vie3.sprite = fullPoint;
            vie4.sprite = fullPoint;
            vie5.sprite = fullPoint;
		}
		else if (a>3)
		{
			vie1.sprite = fullPoint;
            vie2.sprite = fullPoint;
            vie3.sprite = fullPoint;
            vie4.sprite = fullPoint;
            vie5.sprite = emptyPoint;
		}

		else if (a>2)
		{
			vie1.sprite = fullPoint;
            vie2.sprite = fullPoint;
            vie3.sprite = fullPoint;
            vie4.sprite = emptyPoint;
            vie5.sprite = emptyPoint;
		}
		else if (a>1)
		{
			vie1.sprite = fullPoint;
            vie2.sprite = fullPoint;
            vie3.sprite = emptyPoint;
            vie4.sprite = emptyPoint;
            vie5.sprite = emptyPoint;
		}
		else if (a>0)
		{
			vie1.sprite = fullPoint;
            vie2.sprite = emptyPoint;
            vie3.sprite = emptyPoint;
            vie4.sprite = emptyPoint;
            vie5.sprite = emptyPoint;
		}
		else
		{
            vie1.sprite = emptyPoint;
            vie2.sprite = emptyPoint;
            vie3.sprite = emptyPoint;
            vie4.sprite = emptyPoint;
            vie5.sprite = emptyPoint;
        }
    }
}
