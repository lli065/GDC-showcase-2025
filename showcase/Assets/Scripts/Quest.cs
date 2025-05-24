using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed,
    Ended
}

[System.Serializable]
public class Quest : MonoBehaviour
{
    public Dialogue startDialogue;
    public Dialogue inProgressDialogue;
    public Dialogue completedDialogue;
    public Dialogue postQuestDialogue;
    public QuestState state;
}
