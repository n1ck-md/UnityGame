using UnityEngine;
using UnityEngine.InputSystem;

public class BookshelfTrigger : MonoBehaviour
{
    public GameObject interactionPrompt;
    public SlidingPuzzleManager puzzleManager; 

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
        if (!isPlayerInRange) return;

        if (puzzleManager != null)
        {
            puzzleManager.OpenPuzzle();
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false); 
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