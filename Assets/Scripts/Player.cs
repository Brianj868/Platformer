using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpForce = 200;
    [SerializeField] int maxJumps = 2;
    [SerializeField] Transform feet;

    Vector3 startPosition;
    int jumpsRemaining;

    void Start()
    {
        startPosition = transform.position;
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
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
            rigidbody2D.AddForce(Vector2.up * jumpForce);
            jumpsRemaining--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hit = Physics2D.OverlapCircle(feet.position, 0.1f, LayerMask.GetMask("Default"));
        if (hit != null)
        {
            jumpsRemaining = maxJumps;
        }
    }

    internal void ResetToStart()
    {
        transform.position = startPosition;
    }
}