using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private KnockBack knockBack;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (knockBack.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
        if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDir.x < 0)
            spriteRenderer.flipX = true;
    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }
}
