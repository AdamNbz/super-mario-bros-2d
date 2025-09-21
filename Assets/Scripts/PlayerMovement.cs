using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    private Camera cam;
    private Rigidbody2D rb;
    private Vector2 velocity;

    [Header("Tuning")]
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(moveAction.action.ReadValue<Vector2>().x) > 0.25f;
    public bool sliding => (moveAction.action.ReadValue<Vector2>().x > 0f && velocity.x < 0f) || (moveAction.action.ReadValue<Vector2>().x < 0f && velocity.x > 0f);
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);

        if (grounded) GroundedMovement();

        ApplyGravity();
    }

    private void HorizontalMovement()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>();
        velocity.x = Mathf.MoveTowards(velocity.x, move.x * moveSpeed, moveSpeed * Time.deltaTime);

        if (rb.Raycast(Vector2.right * velocity.x)) velocity.x = 0f;

        if (velocity.x > 0f) transform.eulerAngles = Vector3.zero;
        else if (velocity.x < 0f) transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(0f, velocity.y);
        jumping = velocity.y > 0f;

        if (jumpAction.action.WasPressedThisFrame())
        {
            jumping = true;
            velocity.y = jumpForce;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !jumpAction.action.IsPressed();
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 pos = rb.position;
        pos += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = cam.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        pos.x = Mathf.Clamp(pos.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rb.MovePosition(pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}
