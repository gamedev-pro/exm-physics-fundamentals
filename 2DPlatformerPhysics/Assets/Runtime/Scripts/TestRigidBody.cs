using UnityEngine;

public class TestRigidBody : MonoBehaviour
{
    [SerializeField] private float force = 10;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * force * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * force * Time.fixedDeltaTime);
        }
    }
}
