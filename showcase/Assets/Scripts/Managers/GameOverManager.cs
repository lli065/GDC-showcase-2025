using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject thankYouScreen;
    public float screenDelay = 2f;
    public Animator transition;


    void Start()
    {
        if (!GameManager.currentGameManager.wonBossFight)
        {
            gameOverScreen.SetActive(true);
            thankYouScreen.SetActive(false);
        }
        else
        {
            thankYouScreen.SetActive(true);
            gameOverScreen.SetActive(false);
        }
        //StartCoroutine(ShowScreens());
    }

    // public IEnumerator ShowScreens()
    // {
        
    //     yield return new WaitForSeconds(screenDelay);
    //     transition.SetTrigger("Start");
    //     yield return new WaitForSeconds(1f);
    //     transition.SetTrigger("FadeIn");
    //     thankYouScreen.SetActive(true);
    //     gameOverScreen.SetActive(false);
    //     yield return new WaitForSeconds(1f);
    //     yield return new WaitForSeconds(screenDelay);
    //     transition.SetTrigger("Start");
    //     yield return new WaitForSeconds(1f);
    //     transition.SetTrigger("FadeIn");
    //     thankYouScreen.SetActive(false);
    //     creditsScreen.SetActive(true);
    // }
}
