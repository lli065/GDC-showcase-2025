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
    Witch,
    Boss,
    Heaven2
}
public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    private bool playerNearby;
    public Dialogue defaultDialogue;
    public Dialogue hurtDialogue;
    public Quest quest;
    [SerializeField] private ConditionType condition;
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
                    dialogue = quest.completedDialogue != null ? quest.completedDialogue : quest.postQuestDialogue;
                    break;
                case QuestState.Ended:
                    dialogue = quest.postQuestDialogue;
                    break;
            }
        }
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, this);
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
            case ConditionType.Witch:
                return mm.GetMushroomCount(MushroomType.Poison) >= 44;
            case ConditionType.Heaven2:
                return gm.wonBossFight;
        }
        return true;
    }

    public void OnQuestComplete()
    {
        var gm = GameManager.currentGameManager;
        var mm = MushroomManager.Instance;
        if (condition == ConditionType.Heaven && quest.state == QuestState.InProgress)
        {
            gm.talkedToWitch = true;
            moveObjects();
        }
        if (condition == ConditionType.Evilshroom && quest.state == QuestState.Completed)
        {
            moveObjects();
            mm.RemoveMushrooms(5, MushroomType.White);
            mm.RemoveMushrooms(5, MushroomType.Orange);
        }
        if (condition == ConditionType.Chicken && quest.state == QuestState.Completed)
        {
            mm.RemoveMushrooms(5, MushroomType.Heal);
            gm.finishedChickenQuest = true;
        }
        if (condition == ConditionType.Witch)
        {
            quest.state = QuestState.InProgress;
            gm.StartBossFight();
        }
        if (condition == ConditionType.Boss)
        {
            gm.inBossFight = true;
        }
    }

    public void OnDialogueComplete()
    {
        if (quest == null && condition != ConditionType.None)
        {
            OnQuestComplete();
        }
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
        // if (collision.gameObject.CompareTag("PlayerAttack"))
        // {
        //     if (hurtDialogue != null)
        //     {
        //         FindObjectOfType<DialogueManager>().StartDialogue(hurtDialogue, this);
        //     }
        // }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
