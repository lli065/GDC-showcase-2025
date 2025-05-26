using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioClip titleMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;
    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlayMusic(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene.buildIndex);
    }

    public void PlayMusic(int sceneIndex)
    {
        AudioClip audioClip = null;
        switch (sceneIndex)
        {
            case 0:
                audioClip = titleMusic;
                break;
            case 1:
                audioClip = gameMusic;
                break;
            case 2:
                if (GameManager.currentGameManager.inBossFight)
                {
                    audioClip = bossMusic;
                }
                else
                {
                    audioClip = gameMusic;
                }
                break;
            case 3:
                audioClip = gameOverMusic;
                break;
        }
        if (audioClip != null && musicSource.clip != audioClip)
        {
            musicSource.clip = audioClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
