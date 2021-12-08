using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement2D : MonoBehaviour
{
    [SerializeField] private float maxGroundSpeed = 10;

    [SerializeField] private float groundAcc = 10;
    [SerializeField] private float jumpSpeed = 100;

    [SerializeField] private float gravity = 40;

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
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, groundAcc * Time.fixedDeltaTime);

        velocity.y -= gravity * Time.fixedDeltaTime;

        rb.velocity = velocity;
    }

    public void SetMoveInput(float horizontal)
    {
        input.x = Mathf.Clamp(horizontal, -1, 1);
    }

    public void Jump()
    {
        velocity.y = jumpSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity);
    }
}
