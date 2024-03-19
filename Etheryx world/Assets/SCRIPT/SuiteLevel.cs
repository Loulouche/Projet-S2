
using UnityEngine;

public class SuiteLevel : MonoBehaviour
{

public GameObject[] objects;

    void Awake()
    {
        foreach (var element in objects)
        {
            if (element == null)
            {
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(element);
                }
                else
                {
                    Debug.LogWarning("L'objet " + element.name + "ne ser pas consevé");
                }
            }
            else
            {
                Destroy(element);
            }

        }
        
    }

  
}
