using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private int facingDirection = 1; // 1 for right, -1 for left

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) movement.y += 1;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1;
        if (Keyboard.current.dKey.isPressed) movement.x += 1;

        if (movement.x > 0 && facingDirection < 0 || movement.x < 0 && facingDirection > 0)
        {
            flip();
        }


        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    void flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //transform.Rotate(0f, 180f, 0f);
    }
}
