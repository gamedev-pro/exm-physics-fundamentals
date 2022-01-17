using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody2D))]
public class ForceAccumulator : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    [SerializeField] private SimpleForceMode forceMode;

    private SimpleRigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<SimpleRigidbody2D>();
    }

    private void Update()
    {
        rb.AddForce(force, forceMode);
        RefreshEnabledState();
    }

    private void OnValidate()
    {
        enabled = true;
    }

    private void RefreshEnabledState()
    {
        //So aplicamos forca todo update se a forca for gradual
        enabled = forceMode == SimpleForceMode.Force || forceMode == SimpleForceMode.Acceleration;
    }
}
