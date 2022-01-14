using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float killY = -20;

    public SimpleRigidbody2D Rb => GetComponent<SimpleRigidbody2D>();

    private void FixedUpdate()
    {
        if (Rb.Position.y < killY)
        {
            Destroy(gameObject);
        }
    }
}
