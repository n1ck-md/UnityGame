using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private bool isPlayerNearCat = false;
    private PlayerControls controls;
    public DialogueManager dialogueManager;
    public Sprite catSprite;
    public AudioSource pickupSound;

    public static bool catPickedUp = false;

    private bool hasTakenCat = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteractWithCat();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void TryInteractWithCat()
    {
        if (isPlayerNearCat && !hasTakenCat)
        {
            hasTakenCat = true;
            catPickedUp = true;
            dialogueManager.StartDialogue(new string[] { "You picked up the cat!" }, catSprite, "Gato");

            if (pickupSound != null) pickupSound.Play();

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearCat = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearCat = false;
    }
}
