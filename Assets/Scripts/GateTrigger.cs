using UnityEngine;
using UnityEngine.InputSystem;

public class GateTrigger : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;
    public GameObject passwordPanel;
    public Sprite gatePortrait;

    private bool isPlayerInRange = false;
    private PlayerControls controls;

    private bool hasGivenHint = false;

    private string[] hintDialogue = {
        "This gate won't open without the secret word.",
        "Maybe it's hidden somewhere nearby..."
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

        if (!hasGivenHint)
        {
            dialogueManager.StartDialogue(hintDialogue, gatePortrait, "Gate");
            hasGivenHint = true;
        }
        else
        {
            passwordPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
}
