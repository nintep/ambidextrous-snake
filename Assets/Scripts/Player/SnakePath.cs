using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A class for storing the path of a snake
public class SnakePath
{
    private int maxLength;
    private Queue<Waypoint> waypoints;
    private Dictionary<Waypoint, Waypoint> nextWaypoints;

    public int Length => waypoints.Count;

    public SnakePath(int maxLength = 4)
    {
        this.maxLength = maxLength;
        waypoints = new Queue<Waypoint>();
        nextWaypoints = new Dictionary<Waypoint, Waypoint>();
    }

    public void AddWaypoint(Vector2 position)
    {
        Waypoint waypoint = new Waypoint(position);

        if (waypoints.Count != 0)
        {
            Waypoint currentNewest = GetNewestWaypoint();
            nextWaypoints[currentNewest] = waypoint;
        }

        waypoints.Enqueue(waypoint);

        if (waypoints.Count > maxLength)
        {
            Waypoint removed = waypoints.Dequeue();
            nextWaypoints.Remove(removed);
        }
    }

    public void IncreaseWaypointCount(int increment)
    {
        maxLength += increment;
    }

    public Waypoint GetNewestWaypoint()
    {
        return waypoints.Last();
    }

    public Waypoint GetNextWaypoint(Waypoint waypoint)
    {
        // Check if newest or nonexisting waypoint
        if (waypoint == GetNewestWaypoint() || !waypoints.Contains(waypoint))
        {
            return null;
        }

        return nextWaypoints[waypoint];
    }

    public List<Waypoint> GetWaypoints()
    {
        return waypoints.ToList();
    }
}

public class Waypoint
{
    public Vector2 Position {get; private set;}

    public Waypoint(Vector2 position)
    {
        Position = position;
    }
}
