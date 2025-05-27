using UnityEngine;
using UnityEngine.InputSystem;

public class DoorEnter : MonoBehaviour
{
    public Transform teleportLocation;
    private bool isPlayerNear = false;
    private GameObject player;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryEnter();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null;
        }
    }

    private void TryEnter()
    {
        if (isPlayerNear && player != null)
        {
            StartCoroutine(ScreenFader.Instance.FadeOutIn(() =>
            {
                player.transform.position = teleportLocation.position;
            }));
        }
    }
}
