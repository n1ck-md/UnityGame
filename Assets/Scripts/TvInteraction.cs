using UnityEngine;

public class TVInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;
    public Sprite tvPortrait;

    public GameObject leoGameObject;   

    private bool isPlayerInRange = false;
    private bool leoSpawned = false;

    private PlayerControls controls;

    private string[] tvDialogue = {
        "The TV screen flickers with static.",
        "A soft whisper: 'Leo might know more...' ",
        "Maybe try to find him."
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

        dialogueManager.StartDialogue(tvDialogue, tvPortrait, "");

        if (!leoSpawned && leoGameObject != null)
        {
            leoGameObject.SetActive(true); 
            leoSpawned = true;
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
