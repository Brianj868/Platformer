using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1;

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
        Debug.Log($"Velocity = {rigidbody2D.velocity}");
    }
}
