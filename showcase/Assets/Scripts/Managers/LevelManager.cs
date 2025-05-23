using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    
    public void LoadNextScene() {
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator Load(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        GameManager.currentGameManager.currentScene = sceneIndex;
    }
}
