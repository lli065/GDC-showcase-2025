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

    private Queue<string> sentences;
    private bool isTyping = false;
    private string currentSentence;
    private Coroutine typingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update() {
        if (isTalking && Input.GetKeyDown(KeyCode.Space)) {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        isTalking = true;
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
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
        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        StartCoroutine(TypeSentence(sentence));
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