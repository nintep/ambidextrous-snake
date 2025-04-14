using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public event Action ObstacleCollision;
    public event Action<PickUpItem> PickUpCollision;
    public event Action<SnakeSegment> SnakeCollision;

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
