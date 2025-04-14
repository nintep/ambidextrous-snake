using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField]
    private Color leftSnakeColor;

    [SerializeField]
    private Color rightSnakeColor;

    public PlayerType Type {get; private set;}

    public event Action ObstacleCollision;
    public event Action<PickUpItem> PickUpCollision;
    public event Action<SnakeSegment> SnakeCollision;

    public void SetType(PlayerType playerType)
    {
        Type = playerType;

        SpriteRenderer rd = GetComponent<SpriteRenderer>();
        rd.color = playerType == PlayerType.Left ? leftSnakeColor : rightSnakeColor;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PickUpItem>() != null)
        {
            PickUpCollision?.Invoke(collider.gameObject.GetComponent<PickUpItem>());
        }
        else if (collider.gameObject.GetComponent<SnakeSegment>() != null)
        {
            SnakeCollision?.Invoke(collider.gameObject.GetComponent<SnakeSegment>());
        }
        else
        {
            ObstacleCollision?.Invoke();
        }
    }
}
