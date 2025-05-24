using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    None,
    Heaven,
    Chicken,
    Mushroom,
    Evilshroom,
    Witch
}
public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    private bool playerNearby;
    public Dialogue defaultDialogue;
    public Dialogue hurtDialogue;
    [SerializeField] private Quest quest;
    [SerializeField] private ConditionType condition;
    [SerializeField] private List<GameObject> objectsToMove;
    [SerializeField] private List<Vector3> movePositions;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        playerNearby = false;
        visualCue.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
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
        if (quest != null)
        {
            switch (quest.state)
            {
                case QuestState.NotStarted:
                    dialogue = quest.startDialogue;
                    break;
                case QuestState.InProgress:
                    if (CheckCondition())
                    {
                        dialogue = quest.completedDialogue != null ? quest.completedDialogue : quest.postQuestDialogue;
                        quest.state = QuestState.Completed;
                    }
                    else
                    {
                        dialogue = quest.inProgressDialogue;
                    }
                    break;
                case QuestState.Completed:
                    dialogue = quest.completedDialogue != null ? quest.completedDialogue : quest.inProgressDialogue;
                    break;
                case QuestState.Ended:
                    dialogue = quest.postQuestDialogue;
                    break;
            }
        }
        dialogueManager.StartDialogue(dialogue, this);
    }

    public bool CheckCondition()
    {
        if (condition == ConditionType.None) return true;
        var mm = MushroomManager.Instance;
        var gm = GameManager.currentGameManager;
        switch (condition)
        {
            case ConditionType.Chicken:
                return mm.GetMushroomCount(MushroomType.Heal) >= 5;
            case ConditionType.Evilshroom:
                return mm.GetMushroomCount(MushroomType.White) >= 5 && mm.GetMushroomCount(MushroomType.Orange) >= 5;
            case ConditionType.Heaven:
                return gm.finishedChickenQuest;
            case ConditionType.Mushroom:
                return gm.talkedToWitch;
        }
        return true;
    }

    public void OnQuestComplete()
    {
        var gm = GameManager.currentGameManager;
        if (condition == ConditionType.Heaven && quest.state == QuestState.InProgress)
        {
            gm.talkedToWitch = true;
            moveObjects();
        }
        if (condition == ConditionType.Evilshroom && quest.state == QuestState.Completed)
        {
            moveObjects();
        }
        if (condition == ConditionType.Chicken && quest.state == QuestState.Completed)
        {
            MushroomManager.Instance.RemoveMushrooms(5, MushroomType.Heal);
            gm.finishedChickenQuest = true;
        }
    }

    public void OnDialogueComplete()
    {
        if (quest == null) return;
        switch (quest.state)
        {
            case QuestState.NotStarted:
                quest.state = QuestState.InProgress;
                break;
            case QuestState.Completed:
                OnQuestComplete();
                quest.state = QuestState.Ended;
                break;
        }
        OnQuestComplete();
    }

    public void moveObjects()
    {
        for (int i = 0; i < objectsToMove.Count; i++)
        {
            objectsToMove[i].transform.position = movePositions[i];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
        }
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("player attack");
            if (hurtDialogue != null)
            {
                dialogueManager.StartDialogue(hurtDialogue, this);
            }
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
