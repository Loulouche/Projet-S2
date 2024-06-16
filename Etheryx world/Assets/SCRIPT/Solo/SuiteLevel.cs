
using UnityEngine;

public class SuiteLevel : MonoBehaviour
{

    public GameObject[] objects;

    void Awake()
    {
        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
        }
    }

  
}
