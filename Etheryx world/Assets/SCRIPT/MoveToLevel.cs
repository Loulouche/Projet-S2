using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToLevel : MonoBehaviour
{
      public int sceneBuildIndex; 
      public Animator fadeSystem;
 
    // Level move zoned enter, if collider is a player
    // Move game to another scene
    private void OnTriggerEnter2D(Collider2D other) 
{
        if(other.tag == "Player")
        {
            StartCoroutine(loadNextScene());
        }
    }


public IEnumerator loadNextScene()
{ 
    fadeSystem.SetTrigger("FadeIn"); 
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
}

}
