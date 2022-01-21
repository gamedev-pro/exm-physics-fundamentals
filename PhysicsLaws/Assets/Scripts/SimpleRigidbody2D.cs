using UnityEngine;

//Ref: https://answers.unity.com/questions/802181/trying-to-understand-rigidbody-forcemode-derivatio.html
public enum SimpleForceMode
{
    // Aplica uma força gradual ao corpo rigido, em uma frame
    Force,

    // Aplica toda a força nesta frame
    Impulse,

    // = SimpleForceMode.Force, mas desconsidera a massa (F = a)
    Acceleration,

    // = SimpleForceMode.Impulse, mas desconsidera massa (F = a)
    VelocityChange
}

[RequireComponent(typeof(Shape2D))]
public class SimpleRigidbody2D : MonoBehaviour
{
    public Vector2 Velocity;

    [field: SerializeField]
    public float LinearDrag { get; set; }

    public float AngularVelocity;

    [field: SerializeField]
    public float AngularDrag { get; set; }


    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    //Orientation in radians
    public float Orientation
    {
        get => transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        set
        {
            var rot = transform.rotation.eulerAngles;
            rot.z = value * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    private Shape2D Shape => GetComponent<Shape2D>();

    public float InverseMass => Shape.InverseMass;

    public float Mass => Shape.Mass;

    public float MomentOfInertia => Shape.MomentOfInertia;
    public float InverseMomentOfInertia => Shape.InverseMomentOfInertia;


    public Vector2 InstantNetForce { get; private set; }

    public Vector2 NetForce { get; private set; }

    public float InstantNetTorque { get; private set; }

    public float NetTorque { get; private set; }

    private void Awake()
    {
        //TODO: PhysicsWorld2D deve ser um singleton
        var physicsWorld = FindObjectOfType<PhysicsWorld2D>();
        physicsWorld.Register(this);
    }

    private void OnDestroy()
    {
        //TODO: PhysicsWorld2D deve ser um singleton
        var physicsWorld = FindObjectOfType<PhysicsWorld2D>();
        if (physicsWorld != null)
        {
            physicsWorld.Unregister(this);
        }
    }

    public void ResetForces()
    {
        NetForce = InstantNetForce = Vector2.zero;
        NetTorque = InstantNetTorque = 0;
    }

    public void AddForce(Vector2 force, SimpleForceMode mode)
    {
        switch (mode)
        {
            case SimpleForceMode.Force:
                NetForce += force;
                break;
            case SimpleForceMode.Impulse:
                InstantNetForce += force;
                break;
            case SimpleForceMode.Acceleration:
                NetForce += (force / InverseMass);
                break;
            case SimpleForceMode.VelocityChange:
                InstantNetForce += (force / InverseMass);
                break;
            default:
                throw new System.NotImplementedException($"Unexpected ForceMode: {mode}");
        }
    }

    public void AddTorque(float torque, SimpleForceMode mode)
    {
        switch (mode)
        {
            case SimpleForceMode.Force:
                NetTorque += torque;
                break;
            case SimpleForceMode.Impulse:
                InstantNetTorque += torque;
                break;
            case SimpleForceMode.Acceleration:
                NetTorque += (torque / InverseMomentOfInertia);
                break;
            case SimpleForceMode.VelocityChange:
                InstantNetTorque += (torque / InverseMomentOfInertia);
                break;
            default:
                throw new System.NotImplementedException($"Unexpected ForceMode: {mode}");
        }
    }
}
