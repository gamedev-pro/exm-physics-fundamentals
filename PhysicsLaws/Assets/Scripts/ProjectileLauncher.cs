using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10;

    [Tooltip("Tamanho maxima para o vetor de lancamento. Apenas cosmetico.")]
    [SerializeField] private float visualMaxLaunchVectorLength = 2;

    [SerializeField] private float gravity = 9.5f;

    [SerializeField] float simulationDeltaTime = 0.1f;

    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private Transform graphics;
    [SerializeField] private Transform muzzleStart;

    private float referenceInputDistance;

    private Vector2 mouseInputFromLaunchPos;

    private Vector2 LaunchPosition => transform.position;
    private float LaunchSpeedPercent => referenceInputDistance > 0 ? mouseInputFromLaunchPos.magnitude / referenceInputDistance : 0;

    private Camera cam;
    private Camera Camera => cam == null ? cam = Camera.main : cam;

    private bool aiming;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mouseScreenPos = (Vector2)Camera.ScreenToWorldPoint(Input.mousePosition);
            mouseInputFromLaunchPos = mouseScreenPos - LaunchPosition;

            //Setamos a distância de referência quando o jogador de fato clica o botão pela primeira vez
            if (Input.GetMouseButtonDown(0))
            {
                aiming = true;
                //Consideramos que a posição que o Player clico é 50% da magnitude total
                //então a distância de referência tem que ser o dobro (ou 1/0.5f)
                referenceInputDistance = mouseInputFromLaunchPos.magnitude * 2;
            }

            //Girando o canhão para olhar pro vetor de lançamento.
            //Espera que o pivot esteja no começo do canhão
            var zRot = Mathf.Acos(mouseInputFromLaunchPos.x / mouseInputFromLaunchPos.magnitude) * Mathf.Rad2Deg;
            var currentRot = graphics.rotation.eulerAngles;
            graphics.rotation = Quaternion.Euler(currentRot.x, currentRot.y, zRot);
        }

        if (aiming && Input.GetMouseButtonUp(0))
        {
            aiming = false;
            var projectile = Instantiate(projectilePrefab);
            var point = GetLaunchParameters();
            projectile.Rb.Position = point.Position;
            projectile.Rb.Velocity = point.Velocity;
            projectile.Rb.Acceleration = new Vector2(0, -gravity);
        }
    }

    private ParabolicMovementParameters GetLaunchParameters()
    {
        var physicsPoint = new ParabolicMovementParameters();
        physicsPoint.Position = muzzleStart.position;
        physicsPoint.Velocity = mouseInputFromLaunchPos.normalized * LaunchSpeedPercent * maxSpeed;
        physicsPoint.Gravity = gravity;
        return physicsPoint;
    }

    private void OnDrawGizmos()
    {
        if (LaunchSpeedPercent > 0)
        {
            var positions = ParabolicMovementCalculator.SimulateParabolicTrajectory(GetLaunchParameters(), transform.position.y, simulationDeltaTime);
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
