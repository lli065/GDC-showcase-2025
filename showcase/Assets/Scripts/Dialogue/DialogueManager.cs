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

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);
    }

    void Update() {
        if (isTalking && Input.GetKeyDown(KeyCode.Space)) {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        dialogueBox.SetActive(true);
        isTalking = true;
        animator.SetBool("isOpen", true);

        sentences.Clear();

        foreach (DialogueLine line in dialogue.lines) {
            sentences.Enqueue(line);
        }
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        if (isTyping) {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }
        DialogueLine line = sentences.Dequeue();
        currentSentence = line.sentence;
        nameText.text = line.name;
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        isTyping = false;
    }

    public void EndDialogue() {
        animator.SetBool("isOpen", false);
        isTalking = false;
        StartCoroutine(DialogueCooldown());
    }

    IEnumerator DialogueCooldown() {
        canTrigger = false;
        yield return new WaitForSeconds(.2f);
        canTrigger = true;
    }
}