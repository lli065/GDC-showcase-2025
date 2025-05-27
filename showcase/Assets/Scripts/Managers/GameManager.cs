using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager currentGameManager;
    public Animator transition;
    public LevelManager levelManager;

    public Transform playerTransform;
    public Transform kefaTransform;
    public Transform heavenTransform;
    public Vector3 bossFightPosition;
    public bool talkedToWitch = false;
    public bool finishedChickenQuest = false;
    public bool inBossFight = false;
    public bool hasPoisonMushrooms = false;
    public bool wonBossFight = false;
    public int currentScene;
    public Dialogue bossFightDialogue;
    public Vector3 endBossFightPosition;
    public Npc bossNpc;
    public Npc newHeaven;
    public Npc navaeh;
    public Npc death;
    public Vector3 playerPosition;
    public Vector3 heavenPosition;
    public Vector3 kefaPosition;
    public GameObject deathTrigger;
    public GameObject oldHeaven;

    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    

    private void Awake()
    {
        if (currentGameManager == null)
        {
            currentGameManager = this;
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
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void OpenControls()
    {
        controlsMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        levelManager.LoadMenu();
    }

    public void ShowGameOverScreen()
    {
        levelManager.LoadGameOver();
    }

    public void StartBossFight()
    {
        StartCoroutine(BossFightTransition());
    }

    IEnumerator BossFightTransition()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        RemoveAllGhosts();
        playerTransform.position = bossFightPosition;
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<DialogueManager>().StartDialogue(bossFightDialogue, bossNpc);
    }

    public void EndBossFight()
    {
        inBossFight = false;
        playerTransform.GetComponent<PlayerController>().ResetHealth();
        StartCoroutine(BossFightEndTransition());
    }

    IEnumerator BossFightEndTransition()
    {
        navaeh.quest.state = QuestState.InProgress;
        newHeaven.gameObject.transform.position = new Vector3(22.26f, -17.32f, 0);
        oldHeaven.transform.position = new Vector3(-5000, 0, 0);
        RemoveAllGhosts();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        playerTransform.position = endBossFightPosition;
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        newHeaven.TriggerDialogue();
        if (wonBossFight)
        {
            deathTrigger.transform.position = new Vector3(0, 0, 0);
        }
    }

    public IEnumerator StartEndingScene()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        transition.SetTrigger("Start");
        dialogueManager.EndDialogue();
        yield return new WaitForSeconds(1f);
        playerTransform.position = playerPosition;
        kefaTransform.position = kefaPosition;
        heavenTransform.position = heavenPosition;
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        death.TriggerDialogue();
        while (DialogueManager.isTalking)
        {
            yield return null;
        }
        ShowGameOverScreen();
    }

    public void DecreasePlayerHealth()
    {
        playerTransform.GetComponent<PlayerController>().StartDecreasingHealth();
    }

    public void RemoveAllGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }
    }
}
