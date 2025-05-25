using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static bool isTalking = false;
    public static bool canTrigger = true;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    [SerializeField] private GameObject dialogueBox;

    private Queue<DialogueLine> sentences;
    private bool isTyping = false;
    private string currentSentence;
    private Coroutine typingCoroutine;
    private Npc currentNpc;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);
    }

    void Update() {
        if (isTalking && Input.GetKeyDown(KeyCode.E)) {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, Npc npc)
    {
        dialogueBox.SetActive(true);
        isTalking = true;
        animator.SetBool("isOpen", true);
        currentNpc = npc;

        sentences.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        DialogueLine line = sentences.Dequeue();
        currentSentence = line.sentence;
        nameText.text = line.name;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        isTyping = false;
    }

    public void EndDialogue() {
        animator.SetBool("isOpen", false);
        isTalking = false;
        if (currentNpc != null)
        {
            currentNpc.OnDialogueComplete();
            currentNpc = null;
        }
        StartCoroutine(DialogueCooldown());
    }

    IEnumerator DialogueCooldown() {
        canTrigger = false;
        yield return new WaitForSeconds(.2f);
        canTrigger = true;
    }
}