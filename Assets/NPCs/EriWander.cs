using UnityEngine;
using System.Collections;

public class EriWander : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float moveTime = 1f;        
    public float waitTime = 2f;        

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false;
    private Animator animator;
    private Vector2 startPos;

    public Vector2 moveBounds = new Vector2(4, 1.5f); // Half-width si half-height la arie in masuratorile la unity

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPos = transform.position;

        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            isMoving = true;
            moveDirection = GetRandomDirection();

            float elapsed = 0f;
            while (elapsed < moveTime)
            {
                Vector2 newPos = rb.position + moveDirection * moveSpeed * Time.deltaTime;

                // Stay within bounds
                if (Mathf.Abs(newPos.x - startPos.x) <= moveBounds.x &&
                    Mathf.Abs(newPos.y - startPos.y) <= moveBounds.y)
                {
                    // verifca coliziuni
                    RaycastHit2D[] hits = new RaycastHit2D[1];
                    int hitCount = rb.Cast(moveDirection, hits, moveSpeed * Time.deltaTime);

                    if (hitCount == 0)
                    {
                        rb.MovePosition(newPos);
                        animator.SetFloat("MoveX", moveDirection.x);
                        animator.SetFloat("MoveY", moveDirection.y);
                        animator.SetBool("IsMoving", true);
                    }
                    else
                    {
                        break; 
                    }

                    animator.SetFloat("MoveX", moveDirection.x);
                    animator.SetFloat("MoveY", moveDirection.y);
                    animator.SetBool("IsMoving", true);
                }
                else
                {
                    break; 
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            animator.SetBool("IsMoving", false);
            isMoving = false;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector2 GetRandomDirection()
    {
        
        int choice = Random.Range(0, 4);
        return choice switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.zero,
        };
    }
}
