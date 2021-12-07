using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float killY = -20;

    private PhysicsPoint point;

    public void Initialize(PhysicsPoint p)
    {
        point = p;
    }
    private void FixedUpdate()
    {
        PhysicsUtils.SimulatePhysicsPoint(point, Time.fixedDeltaTime);
        transform.position = point.Position;

        if (transform.position.y < killY)
        {
            Destroy(gameObject);
        }
    }
}
