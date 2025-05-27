using UnityEngine;
using UnityEngine.InputSystem;

public class LeoInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;
    public Sprite leoPortrait;

    private bool isPlayerInRange = false;
    private PlayerControls controls;

    private bool hasTalkedOnce = false;
    private bool gaveHint = false;

    public GameObject LeftBookGameObject;
    private bool LeftBookSpawned = false;

    private string[] firstDialogue = {
        "Whoa... You actually heard the TV?",
        "You're pretty special aren't you?"
    };

    private string[] hintDialogue = {
        "Try to interact with the bookshelf on the left",
        "Not sure if it's real or but it's worth checking."
    };

    private string[] afterHintDialogue = {
        "Did you check it?",
        "Try to go on the right one then, my friend did put something there ))",
        "That's all I know..."
    };

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void TryInteract()
    {
        if (!isPlayerInRange) return;

        if (!hasTalkedOnce)
        {
            dialogueManager.StartDialogue(firstDialogue, leoPortrait, "Leo");
            hasTalkedOnce = true;
        }
        else if (!gaveHint)
        {
            dialogueManager.StartDialogue(hintDialogue, leoPortrait, "Leo");
            gaveHint = true;

            if (!LeftBookSpawned && LeftBookGameObject != null)
            {
                LeftBookGameObject.SetActive(true);  // Activate Leo
                LeftBookSpawned = true;
            }

        }
        else
        {
            dialogueManager.StartDialogue(afterHintDialogue, leoPortrait, "Leo");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}