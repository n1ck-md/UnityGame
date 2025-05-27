using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;          
    public Image portraitImage;            
    public AudioSource blipAudio;          
    public TMP_Text nameText;

    private string[] lines;
    private int currentLine = 0;
    private System.Action onDialogueComplete;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => OnAdvance();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    public void StartDialogue(string[] dialogueLines, Sprite portrait, string speakerName, System.Action onComplete = null)
    {
        lines = dialogueLines;
        currentLine = 0;
        onDialogueComplete = onComplete; //tine minte callback

        nameText.text = speakerName;
        portraitImage.sprite = portrait;
        dialogueBox.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(lines[currentLine]));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;

            
            if (blipAudio != null)
            {
                blipAudio.Stop(); 
                blipAudio.Play(); 
            }

            
            yield return new WaitForSeconds(0.05f); //typing speed-ul
        }

        isTyping = false;
    }

    private void OnAdvance()
    {
        if (!dialogueBox.activeSelf || lines == null || currentLine >= lines.Length) return;

        
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = lines[currentLine];
            isTyping = false;
        }
        else
        {
            currentLine++;
            if (currentLine < lines.Length)
            {
                
                typingCoroutine = StartCoroutine(TypeLine(lines[currentLine]));
            }
            else
            {
                
                dialogueBox.SetActive(false);
                lines = null; 
                onDialogueComplete?.Invoke(); 
                onDialogueComplete = null;
            }
        }
    }
}
