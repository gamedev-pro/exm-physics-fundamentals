using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement2D : MonoBehaviour
{
    [SerializeField] private float maxGroundSpeed = 10;

    [SerializeField] private float timeToMaxGroundSpeedSeconds = 0.2f;
    [SerializeField] private float jumpHeight = 100;

    [SerializeField] private float timeToJumpApex = 0.5f;

    private float GroundAcceleration => maxGroundSpeed / timeToMaxGroundSpeedSeconds;
    private float JumpSpeed => 2 * jumpHeight / timeToJumpApex;
    private float Gravity => -JumpSpeed / timeToJumpApex;

    private Rigidbody2D rb;

    private Vector2 velocity;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var targetVelocityX = input.x * maxGroundSpeed;
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, GroundAcceleration * Time.fixedDeltaTime);

        velocity.y += Gravity * Time.fixedDeltaTime;

        rb.velocity = velocity;
    }

    public void SetMoveInput(float horizontal)
    {
        input.x = Mathf.Clamp(horizontal, -1, 1);
    }

    public void Jump()
    {
        velocity.y = JumpSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity);
    }
}
