using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterMovement2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxGroundSpeed = 10;

    [SerializeField] private float timeToMaxGroundSpeedSeconds = 0.2f;

    [SerializeField] private LayerMask groundMask;

    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 100;

    [SerializeField] private float timeToJumpApex = 0.5f;

    private float GroundAcceleration => maxGroundSpeed / timeToMaxGroundSpeedSeconds;
    private float JumpSpeed => 2 * jumpHeight / timeToJumpApex;
    private float Gravity => -JumpSpeed / timeToJumpApex;

    private Rigidbody2D rb;

    private Collider2D coll;

    private Vector2 velocity;
    private Vector2 input;

    private bool isGrounded;

    private Vector2 Position => transform.position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        var targetVelocityX = input.x * maxGroundSpeed;
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, GroundAcceleration * Time.fixedDeltaTime);

        velocity.y += Gravity * Time.fixedDeltaTime;

        CheckVerticalCollision(ref velocity);

        rb.velocity = velocity;

        Debug.DrawRay(Position + Vector2.up * coll.bounds.extents.y, velocity.normalized * 2);
    }

    private void CheckVerticalCollision(ref Vector2 velocity)
    {
        const float rayLength = 0.05f;

        isGrounded = false;
        if (velocity.y <= 0)//Check collision bottom        
        {
            {
                var origin = Position + Vector2.down * rayLength * 0.5f;
                var size = new Vector2(coll.bounds.extents.x, rayLength);

                var hit = Physics2D.OverlapBox(
                    origin,
                    size,
                    0,
                    groundMask);

                if (hit)
                {
                    isGrounded = true;
                    velocity.y = 0;
                }

                DebugUtils.DrawBox(origin, size, hit ? Color.red : Color.white);
            }
        }
        else//Check collision top
        {
            var origin = Position + Vector2.up * (coll.bounds.size.y + rayLength * 0.5f);
            var size = new Vector2(coll.bounds.extents.x, rayLength);

            var hit = Physics2D.OverlapBox(
                origin,
                size,
                0,
                groundMask);

            if (hit)
            {
                velocity.y = 0;
            }

            DebugUtils.DrawBox(origin, size, hit ? Color.red : Color.white);
        }
    }

    public void SetMoveInput(float horizontal)
    {
        input.x = Mathf.Clamp(horizontal, -1, 1);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y = JumpSpeed;
        }
    }
}
