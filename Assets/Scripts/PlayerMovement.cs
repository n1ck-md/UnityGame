using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerControls controls;
    private Rigidbody2D rb;


    public bool canMove = true;

    private bool isForcedMove = false;
    private Vector2 forcedDirection = Vector2.zero;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx =>
        {
            if (canMove)
                moveInput = ctx.ReadValue<Vector2>();
        };
        controls.Player.Move.canceled += ctx =>
        {
            if (canMove)
                moveInput = Vector2.zero;
        };
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 currentInput = (canMove) ? moveInput : (isForcedMove ? forcedDirection : Vector2.zero);
        if (currentInput != Vector2.zero)
            currentInput.Normalize();

        animator.SetFloat("MoveX", currentInput.x);
        animator.SetFloat("MoveY", currentInput.y);
        animator.SetBool("IsMoving", currentInput != Vector2.zero);
    }

    private void FixedUpdate()
    {
        Vector2 currentInput = (canMove) ? moveInput : (isForcedMove ? forcedDirection : Vector2.zero);
        rb.MovePosition(rb.position + currentInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void ForceMove(Vector2 direction)
    {
        isForcedMove = true;
        forcedDirection = direction.normalized;
    }

    public void StopForcedMove()
    {
        isForcedMove = false;
        forcedDirection = Vector2.zero;
    }
}
