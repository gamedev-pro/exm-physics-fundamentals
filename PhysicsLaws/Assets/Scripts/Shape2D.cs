using UnityEngine;
using UnityEngine.Assertions;

public abstract class Shape2D : MonoBehaviour
{
    [SerializeField] private float mass = 1;

    public float InverseMass { get; private set; }

    public float Mass => mass;

    public float MomentOfInertia { get; private set; }
    public float InverseMomentOfInertia { get; private set; }

    public abstract Vector2 CenterOfMass { get; }

    private SimpleRigidbody2D Rb => GetComponent<SimpleRigidbody2D>();

    private void Awake()
    {
        UpdateMassAndMomentOfInertia();
    }

    private void UpdateMassAndMomentOfInertia()
    {
        Assert.IsFalse(Mathf.Approximately(mass, 0), "0 mass not accepted");
        InverseMass = Mathf.Approximately(mass, 0) ? 0 : 1.0f / mass;
        MomentOfInertia = CalculateMomentOfInertia();
        InverseMomentOfInertia = 1.0f / MomentOfInertia;
    }

    private void OnValidate()
    {
        UpdateMassAndMomentOfInertia();
    }

    protected abstract float CalculateMomentOfInertia();
}