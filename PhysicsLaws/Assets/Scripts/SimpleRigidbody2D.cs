using UnityEngine;
using UnityEngine.Assertions;

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

public class SimpleRigidbody2D : MonoBehaviour
{

    [SerializeField] private float mass = 1;

    public Vector2 Velocity;

    [field: SerializeField]
    public float LinearDrag { get; set; }

    public float AngularVelocity;

    public float AngularAcceleration;

    [field: SerializeField]
    public float AngularDrag { get; set; }


    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public float Orientation
    {
        get => transform.rotation.eulerAngles.z;
        set
        {
            var rot = transform.rotation.eulerAngles;
            rot.z = value;
            transform.rotation = Quaternion.Euler(rot);
        }
    }

    public float InverseMass { get; private set; }

    public float Mass => mass;

    public Vector2 InstantNetForce { get; private set; }

    public Vector2 NetForce { get; private set; }

    private void Awake()
    {
        UpdateInverseMass();
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

    private void OnValidate()
    {
        UpdateInverseMass();
    }

    private void UpdateInverseMass()
    {
        Assert.IsFalse(Mathf.Approximately(mass, 0), "0 mass not accepted");
        InverseMass = Mathf.Approximately(mass, 0) ? 0 : 1.0f / mass;
    }

    public void ResetForces()
    {
        NetForce = InstantNetForce = Vector2.zero;
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
}
