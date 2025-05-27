using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public DialogueManager dialogueManager;
    public Sprite eriPortrait;

    private bool isPlayerInRange = false;
    private PlayerControls controls;
    private bool hasStartedDialogue = false;
    private bool hasHadFullDialogue = false;
    private bool questGiven = false;
    private bool questCompleted = false;

    private string[] introDialogue = {
        "Hi there! I'm Mango.",
        "The sun feels nice today, doesn't it?",
        "Be careful wandering off too far!"
    };

    private string[] questDialogue = {
        "Hey! Can you help me find my lost cat?",
        "She dissapeared last week, I cannot find her...",
        "Last time I saw her was when we went on the bridge"
    };

    private string[] thanksDialogue = {
        "You found her! Thank you so much!",
        "You're the best!"
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

        // Already completed?
        if (questCompleted)
        {
            dialogueManager.StartDialogue(new string[] { "Thanks again for finding her!" }, eriPortrait, "Mango");
            return;
        }

        // Checker daca quest-ul completat (cand se ia matza)
        if (CatInteraction.catPickedUp && !questCompleted)
        {
            dialogueManager.StartDialogue(thanksDialogue, eriPortrait,"Mango");
            questCompleted = true;
            QuestManager.Instance.CompleteQuest("Find the Lost Cat");
            return;
        }

        // Prima interactiune si iti da quest-ul
        if (!questGiven)
        {
            dialogueManager.StartDialogue(questDialogue, eriPortrait, "Mango");
            questGiven = true;

            Quest newQuest = new Quest(
                "Find the Lost Cat",
                "Help Mango find her missing cat. You might need to interact around to find her...",
                new System.Collections.Generic.List<string> { "Find the cat" },
                50,
                10
            );

            QuestManager.Instance.AddQuest(newQuest);
            return;
        }

        // Dupa ce o dat quest-ul dar nu completat
        if (!questCompleted)
        {
            dialogueManager.StartDialogue(new string[] {
                "Still looking for her? I hope she's okay..."
            }, eriPortrait, "Mango");
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
