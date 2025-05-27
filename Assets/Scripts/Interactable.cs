using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;

    public InteractionData interactionData;  

    private bool isPlayerInRange = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteract();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void TryInteract()
    {
        if (!isPlayerInRange || interactionData == null || interactionData.interactionTexts.Length == 0)
            return;

        dialogueManager.StartDialogue(interactionData.interactionTexts, interactionData.portrait, "");
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
