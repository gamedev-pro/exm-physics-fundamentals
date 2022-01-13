using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10;

    [SerializeField] private float gravity = 9.5f;

    [SerializeField] float simulationDeltaTime = 0.1f;

    [Space]
    [Header("Visuals")]
    [Tooltip("Tamanho maxima para o vetor de lancamento. Apenas cosmetico.")]
    [SerializeField] private float visualMaxLaunchVectorLength = 2;
    [SerializeField] private Transform graphics;
    [SerializeField] private Transform muzzleStart;

    private float referenceInputDistance;

    private Vector2 muzzleToMousePos;

    private Vector2 LaunchPosition => muzzleStart.position;
    private float LaunchSpeedPercent => Mathf.Clamp01(muzzleToMousePos.magnitude / referenceInputDistance);

    private Camera cam;
    private Camera Camera => cam == null ? cam = Camera.main : cam;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mouseScreenPos = (Vector2)Camera.ScreenToWorldPoint(Input.mousePosition);

            muzzleToMousePos = mouseScreenPos - LaunchPosition;
            muzzleToMousePos = new Vector2(Mathf.Max(0, muzzleToMousePos.x), Mathf.Max(0, muzzleToMousePos.y));

            //Setamos a distância de referência quando o jogador de fato clica o botão pela primeira vez
            if (Input.GetMouseButtonDown(0))
            {
                //Consideramos que a posição que o Player clicou é 50% da magnitude total
                //então a distância de referência tem que ser o dobro (ou 1/0.5f)
                referenceInputDistance = muzzleToMousePos.magnitude * 2;
            }

            //Girando o canhão para olhar pro vetor de lançamento.
            //Assume que o pivot esteja no começo do canhão
            var originToMousePos = mouseScreenPos - (Vector2)transform.position;
            originToMousePos = new Vector2(Mathf.Max(0, originToMousePos.x), Mathf.Max(0, originToMousePos.y));
            if (originToMousePos.x > 0)
            {
                var zRot = Mathf.Acos(originToMousePos.x / originToMousePos.magnitude) * Mathf.Rad2Deg;
                var currentRot = graphics.rotation.eulerAngles;
                graphics.rotation = Quaternion.Euler(currentRot.x, currentRot.y, zRot);
            }
        }
    }

    private SimpleRigidBody2D NewRb2D()
    {
        var rb = new SimpleRigidBody2D();
        rb.Position = muzzleStart.position;
        rb.Velocity = muzzleToMousePos.normalized * LaunchSpeedPercent * maxSpeed;
        rb.Acceleration = Vector2.down * gravity;
        return rb;
    }

    private void OnDrawGizmos()
    {
        if (Input.GetMouseButton(0))
        {
            var positions = PhysicsUtils.SimulateParabolicTrajectory(NewRb2D(), transform.position.y, simulationDeltaTime);
            Gizmos.color = Color.blue;
            foreach (var pos in positions)
            {
                Gizmos.DrawSphere(pos, 0.1f);
            }

            Gizmos.color = Color.red;
            var launchVectorLength = LaunchSpeedPercent * visualMaxLaunchVectorLength;
            GizmosUtils.DrawVector(muzzleStart.position, graphics.right * launchVectorLength, 5);
        }
    }
}
