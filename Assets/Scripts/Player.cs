using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpVelocity = 10;
    [SerializeField] int maxJumps = 2;
    [SerializeField] Transform feet;
    [SerializeField] float downPull = 5;
    [SerializeField] float maxJumpDuration = 0.1f;

    Vector3 startPosition;
    int jumpsRemaining;
    float fallTimer;
    float jumpTimer;
    
    void Start()
    {
        startPosition = transform.position;
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        var hit = Physics2D.OverlapCircle(feet.position, 0.1f, LayerMask.GetMask("Default"));
        bool isGrounded = hit != null;

        var horizontal = Input.GetAxis("Horizontal") * speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();

        if (Mathf.Abs(horizontal) >= 1)
        {
            rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
            Debug.Log($"Velocity = {rigidbody2D.velocity}");
        }

        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        if (horizontal != 0)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }

        if (Input.GetButtonDown("Fire1") && jumpsRemaining > 0)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpVelocity);
            jumpsRemaining--;
            fallTimer = 0;
            jumpTimer = 0;
        }
        else if (Input.GetButton("Fire1") && jumpTimer <= maxJumpDuration)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpVelocity);
            fallTimer = 0;
            jumpTimer += Time.deltaTime;
        }

        if (isGrounded)
        {
            fallTimer = 0;
            jumpsRemaining = maxJumps;
        }
        else
        {
            fallTimer += Time.deltaTime;
            var downForce = downPull * fallTimer * fallTimer;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y - downForce);
        }
    }

    internal void ResetToStart()
    {
        transform.position = startPosition;
    }
}