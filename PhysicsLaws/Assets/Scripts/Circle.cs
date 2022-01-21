using UnityEngine;

public class Circle : Shape2D
{
    [SerializeField] private float radius;

    public override Vector2 CenterOfMass => throw new System.NotImplementedException();

    protected override float CalculateMomentOfInertia()
    {
        return 0.5f * Mass * radius * radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}