using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerType playerType;

    [SerializeField]
    private float movementSpeed = 2.0f;

    [SerializeField]
    private bool startAutomatically = false;

    private bool gameStarted;
    private bool playerAlive;
    private bool movementActive;
    private Vector2 startDirection = Vector2.right;

    private PlayerInput input;
    private InputAction moveAction;
    private Vector2 moveDirection;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        if (input == null)
        {
            Debug.LogError("Player input component missing");
            return;
        }

        moveAction = playerType == PlayerType.Left ? input.actions["MoveLeftPawn"] : input.actions["MoveRightPawn"];
        moveAction.Enable();

        moveDirection = startDirection;
        movementActive = false;

        if (startAutomatically)
        {
            StartMovement();
        }
    }

    public void SetPlayerType(PlayerType type)
    {
        moveAction.Disable();
        playerType = type;
        moveAction = playerType == PlayerType.Left ? input.actions["MoveLeftPawn"] : input.actions["MoveRightPawn"];
        moveAction.Enable();
    }

    public void SetStartDirection(Vector2 direction)
    {
        startDirection = direction;
    }

    public void StartMovement()
    {
        movementActive = true;
        moveDirection = startDirection;
    }

    public void EndMovement()
    {
        movementActive = false;
    }

    public void AddMoveInput(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            moveDirection = input.normalized;
        }
        transform.position += (Vector3)moveDirection * movementSpeed * Time.deltaTime;
    }

    private void Update()
    {
        if (!movementActive)
        {
            return;
        }
        AddMoveInput(moveAction.ReadValue<Vector2>());
    }
}
