using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public event Action SnakeDied;

    [SerializeField]
    private SnakeSegment snakeSegmentPrefab;

    [SerializeField]
    private PlayerMovement playerMovement;

    private SnakePath path;
    private List<SnakeSegment> segments;
    private float segmentRadius = 0.25f;
    private Dictionary<int, Waypoint> segmentWaypoints;

    private Vector2 latestHeadPosition;
    private float waypointDistance;

    private bool alive;

    public SnakePath GetPath => path;
    
    private void Awake()
    {
        path = new SnakePath();

        segments = new List<SnakeSegment>();
        SnakeSegment head = GetComponentInChildren<SnakeSegment>();
        if (head == null)
        {
            Debug.LogError("No head found for player");
            return;
        }

        segments.Add(head);
        head.ObstacleCollision += OnObstacleHit;
        head.PickUpCollision += OnPickUpCollected;

        segmentWaypoints = new Dictionary<int, Waypoint>();
        waypointDistance = 0.9f * segmentRadius;
    }

    private void Start()
    {
        latestHeadPosition = transform.position;
        path.AddWaypoint(latestHeadPosition);

        alive = true;
    }

    private void Update()
    {
        if (!alive)
        {
            return;
        }

        if (Vector2.Distance(path.GetNewestWaypoint().Position, transform.position) > waypointDistance)
        {
            path.AddWaypoint(transform.position);
        }

        MoveSegments();
        latestHeadPosition = transform.position;
    }

    private void MoveSegments()
    {
        // Update each tail segment
        for (int i = 1; i < segments.Count; i++)
        {
            SnakeSegment segment = segments[i];
            Waypoint waypoint = segmentWaypoints[i];

            // Update segment's waypoint if current target waypoint was reached
            if (Vector2.Distance(waypoint.Position, segment.transform.position) < 0.01f)
            {
                Waypoint newWaypoint = path.GetNextWaypoint(waypoint);
                if (newWaypoint != null)
                {
                    segmentWaypoints[i] = newWaypoint;
                    waypoint = newWaypoint;
                }
                else
                {
                    Debug.LogError("Next waypoint not found");
                    segmentWaypoints[i] = path.GetNewestWaypoint();
                    waypoint = newWaypoint;
                }
            }

            // Calculate movement
            Vector2 segmentPosition = segment.transform.position;
            Vector2 nextSegmentPosition = segments[i - 1].transform.position;

            float moveDistance = Vector2.Distance(transform.position, latestHeadPosition);

            float dToWaypoint = Vector2.Distance(segmentPosition, waypoint.Position);
            float dWaypointToNextSegment = Vector2.Distance(waypoint.Position, nextSegmentPosition);

            // Slow down segments that are getting too close to the previous segment
            if (dToWaypoint + dWaypointToNextSegment < 1.95 * segmentRadius)
            {
                moveDistance = 0;
            }

            // Give speed boost to segments, if they are lagging behind
            if (dToWaypoint + dWaypointToNextSegment > 2.05 * segmentRadius)
            {
                moveDistance += 0.2f * segmentRadius;
            }

            // Don't move segment past waypoint
            if (dToWaypoint < moveDistance)
            {
                moveDistance = dToWaypoint;
            }

            Vector2 moveDirection = (waypoint.Position - segmentPosition).normalized;
            segment.transform.position += (Vector3)(moveDistance * moveDirection);

            // If segment is too far from next segment, skip to waypoint
            if (Vector2.Distance(segment.transform.position, nextSegmentPosition) > 2.2 * segmentRadius)
            {
                segment.transform.position = waypoint.Position;
            }

        }
    }

    private IEnumerator AddSegment()
    {
        // Store position where new segment will be added
        Vector2 addPosition = segments[0].transform.position;
        Vector2 tailPosition = segments.Last().transform.position;

        // Wait until tail enters addition position
        while (Vector2.Distance(addPosition, tailPosition) >= segmentRadius)
        {
            tailPosition = segments.Last().transform.position;
            yield return null;
        }

        // Wait until tail exits addition position
        while (Vector2.Distance(addPosition, tailPosition) <= 1.5f * segmentRadius)
        {
            tailPosition = segments.Last().transform.position;
            yield return null;
        }

        // Add new segment
        SnakeSegment newSegment = GameObject.Instantiate(snakeSegmentPrefab);
        newSegment.transform.position = addPosition;
        newSegment.transform.localScale = new Vector3(2 * segmentRadius, 2 * segmentRadius, 1);
        segments.Add(newSegment);

        newSegment.ObstacleCollision += OnObstacleHit;
        newSegment.SnakeCollision += (SnakeSegment hit) => OnSnakeHit(segments.IndexOf(newSegment), hit);

        // Set waypoint for new segment
        if (segments.Count == 2)
        {
            // If adding first tail segment, use latest waypoint
            segmentWaypoints[segments.Count - 1] = path.GetNewestWaypoint();
        }
        else
        {
            // Set waypoint for new segment as the previous last segment's waypoint
            segmentWaypoints[segments.Count - 1] = segmentWaypoints[segments.Count - 2];
        }

        // Increase number of waypoints stored in snake path
        int increment = (int)Math.Ceiling(2 * segmentRadius / waypointDistance);
        path.IncreaseWaypointCount(increment);
        
    }

    private void OnObstacleHit()
    {
        playerMovement.EndMovement();
        alive = false;
        SnakeDied?.Invoke();
    }

    private void OnPickUpCollected(PickUpItem item)
    {
        GameObject.Destroy(item.gameObject);
        StartCoroutine(AddSegment());
    }

    private void OnSnakeHit(int sourceSegment, SnakeSegment hitSegment)
    {
        // Check if hit self
        if (segments.Contains(hitSegment))
        {
            //Ignore hits between adjacent segments
            int hitIndex = segments.IndexOf(hitSegment);
            if (Math.Abs(sourceSegment - hitIndex) > 1)
            {
                OnObstacleHit();
            }
        }
        else
        {
            OnObstacleHit();
        }
    }
}
