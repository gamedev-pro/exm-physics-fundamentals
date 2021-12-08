using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    public bool WantsToJump => Input.GetKeyDown(KeyCode.Space);
}
