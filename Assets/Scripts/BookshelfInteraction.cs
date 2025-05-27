using UnityEngine;

public class BookshelfInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;
    public Sprite portrait; 
    public PoemDisplay poemDisplay;

    private bool isPlayerInRange = false;
    private bool hasReadPoem = false;

    private PlayerControls controls;

    private string[] bookshelfDialogue = {
        "There's something tucked between the pages...",
        "It looks like a letter... or a poem?"
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
        dialogueManager.StartDialogue(bookshelfDialogue, portrait, "Narrator", () =>
        {
            if (!hasReadPoem)
            {
                poemDisplay.ShowPoem(true); 
                hasReadPoem = true;
            }
            else
            {
                poemDisplay.ShowPoem(false); 
            }
        });
    }

    private void ShowPoem()
    {
        if (poemDisplay != null)
        {
            poemDisplay.ShowPoem();
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
