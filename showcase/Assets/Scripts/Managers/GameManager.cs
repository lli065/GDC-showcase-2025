using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager currentGameManager;

    public Transform playerTransform;
    public bool talkedToWitch = false;
    public bool finishedChickenQuest = false;
    public int currentScene;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
