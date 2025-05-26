using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager currentGameManager;
    public Animator transition;

    public Transform playerTransform;
    public Vector3 bossFightPosition;
    public bool talkedToWitch = false;
    public bool finishedChickenQuest = false;
    public bool inBossFight = false;
    public bool hasPoisonMushrooms = false;
    public int currentScene;
    public Dialogue bossFightDialogue;
    public Npc bossNpc;

    public bool isPaused = false;
    public GameObject pauseMenu;

    private void Awake()
    {
        if (currentGameManager == null)
        {
            currentGameManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false; 
    }

    public void BackToMenu()
    {

    }

    public void OpenSettings()
    {
        
    }

    public void StartBossFight()
    {
        StartCoroutine(BossFightTransition());
    }

    IEnumerator BossFightTransition()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        playerTransform.position = bossFightPosition;
        FindObjectOfType<DialogueManager>().StartDialogue(bossFightDialogue, bossNpc);
    }
}
