using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    None,
    TalkedToWitch
}
public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    public Dialogue defaultDialogue;
    public Dialogue conditionalDialogue;
    private bool playerNearby;
    [SerializeField] private ConditionType condition = ConditionType.None;
    [SerializeField] private bool changeGameState = false;
    [SerializeField] private ConditionType stateToChange = ConditionType.None;
    [SerializeField] private List<GameObject> objectsToMove;
    [SerializeField] private List<Vector3> movePositions;

    private void Awake()
    {
        playerNearby = false;
        visualCue.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) &&
        !DialogueManager.isTalking && DialogueManager.canTrigger)
        {
            TriggerDialogue();
        }
        if (playerNearby && !DialogueManager.isTalking)
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    public void TriggerDialogue()
    {
        Dialogue dialogue = defaultDialogue;
        if (condition != ConditionType.None && CheckCondition())
        {
            dialogue = conditionalDialogue;
        }
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, this);
    }

    private bool CheckCondition()
    {
        var gm = GameManager.currentGameManager;
        switch (condition)
        {
            case ConditionType.TalkedToWitch:
                return gm.talkedToWitch;
            default:
                return false;
        }
    }

    public void OnDialogueComplete()
    {
        if (!changeGameState) return;
        var gm = GameManager.currentGameManager;
        switch (stateToChange)
        {
            case ConditionType.TalkedToWitch:
                gm.talkedToWitch = true;
                for (int i = 0; i < objectsToMove.Count; i++)
                {
                    objectsToMove[i].transform.position = movePositions[i];
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
