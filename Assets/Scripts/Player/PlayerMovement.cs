using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public event Action playerDied;

    [SerializeField]
    private float movementSpeed = 10.0f;
    public PlayerType playerType;

    private List<SnakeSegment> segments;
    private bool gameStarted;
    private bool playerAlive;
    private Vector2 startDirection;


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

        startDirection = Vector2.right;
        moveDirection = startDirection;

        segments = new List<SnakeSegment>();
        SnakeSegment head = GetComponentInChildren<SnakeSegment>();
        if (head == null)
        {
            Debug.LogError("No head found for player");
            return;
        }

        segments.Add(head);
        head.SegmentCollision += OnObstacleHit;

        playerAlive = true;
        gameStarted = false;
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
        gameStarted = true;
        moveDirection = startDirection;
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
        if (!playerAlive || !gameStarted)
        {
            return;
        }

        AddMoveInput(moveAction.ReadValue<Vector2>());
    }

    private void OnObstacleHit()
    {
        Debug.Log("collision");
        playerAlive = false;
    }
}
