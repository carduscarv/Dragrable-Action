using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void GoToScene(string scene){

        SceneManager.LoadScene(scene);
    }

    public void GoToSceneDelay(string scene){
        // Invoke("GoToScene", 3);
        StartCoroutine(Delay(scene, 3));

    }

    IEnumerator Delay(string text, float delay)
    {
        yield return new WaitForSeconds(delay);
        GoToScene(text);
    
    }

    public void Quit(){
        Application.Quit();
    }
}
