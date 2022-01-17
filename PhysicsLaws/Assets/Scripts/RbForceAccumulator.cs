using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RbForceAccumulator : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    [SerializeField] private ForceMode2D forceMode;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        enabled = forceMode == ForceMode2D.Force;
    }
}
