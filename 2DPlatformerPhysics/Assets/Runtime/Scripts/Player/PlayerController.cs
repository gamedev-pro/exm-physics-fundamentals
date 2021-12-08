using UnityEngine;

[RequireComponent(typeof(CharacterMovement2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private CharacterMovement2D characterMovement;
    private PlayerInput playerInput;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        characterMovement.SetMoveInput(playerInput.MoveInput.x);
        if (playerInput.WantsToJump)
        {
            characterMovement.Jump();
        }

        if (!playerInput.IsJumpButtonHeld)
        {
            characterMovement.AbortJump();
        }
    }
}
