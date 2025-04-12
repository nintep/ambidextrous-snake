using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public event Action SegmentCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SegmentCollision?.Invoke();
    }
}
