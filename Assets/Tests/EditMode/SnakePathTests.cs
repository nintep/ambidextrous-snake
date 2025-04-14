using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using System.Linq;


public class SnakePathTests
{
    [Test, Timeout(5000)]
    public void SnakePathStoresAddedWaypoints()
    {
        SnakePath path = new SnakePath(5);

        Assert.AreEqual(0, path.GetWaypoints().Count);

        path.AddWaypoint(Vector2.zero);
        path.AddWaypoint(Vector2.one);
        path.AddWaypoint(Vector2.left);

        List<Vector2> waypoints = path.GetWaypoints().Select(wp => wp.Position).ToList();

        Assert.AreEqual(3, waypoints.Count);
        Assert.Contains(Vector2.zero, waypoints);
        Assert.Contains(Vector2.one, waypoints);
        Assert.Contains(Vector2.left, waypoints);
    }

    [Test, Timeout(5000)]
    public void SnakePathDeletesOldWaypoints()
    {
        SnakePath path = new SnakePath(3);

        Assert.AreEqual(0, path.GetWaypoints().Count);

        path.AddWaypoint(Vector2.zero);
        path.AddWaypoint(Vector2.one);
        path.AddWaypoint(Vector2.left);

        Assert.AreEqual(3, path.GetWaypoints().Count);

        path.AddWaypoint(Vector2.right);
        path.AddWaypoint(Vector2.up);

        List<Vector2> waypoints = path.GetWaypoints().Select(wp => wp.Position).ToList();

        Assert.AreEqual(3, waypoints.Count);
        Assert.Contains(Vector2.left, waypoints);
        Assert.Contains(Vector2.right, waypoints);
        Assert.Contains(Vector2.up, waypoints);
    }

    [Test, Timeout(5000)]
    public void SnakePathIncreaseWaypointCount()
    {
        SnakePath path = new SnakePath(3);

        Assert.AreEqual(0, path.GetWaypoints().Count);

        path.AddWaypoint(Vector2.zero);
        path.AddWaypoint(Vector2.one);
        path.AddWaypoint(Vector2.left);

        Assert.AreEqual(3, path.GetWaypoints().Count);

        path.AddWaypoint(Vector2.right);

        Assert.AreEqual(3, path.GetWaypoints().Count);

        path.IncreaseWaypointCount(2);

        path.AddWaypoint(Vector2.right);
        path.AddWaypoint(Vector2.up);
        path.AddWaypoint(Vector2.down);

        Assert.AreEqual(5, path.GetWaypoints().Count);

        path.IncreaseWaypointCount(1);

        path.AddWaypoint(Vector2.up);
        path.AddWaypoint(Vector2.down);

        Assert.AreEqual(6, path.GetWaypoints().Count);
    }

    [Test, Timeout(5000)]
    public void SnakePathGetNewestWaypoint()
    {
        SnakePath path = new SnakePath(3);

        path.AddWaypoint(Vector2.zero);

        Assert.AreEqual(Vector2.zero, path.GetNewestWaypoint().Position);

        path.AddWaypoint(Vector2.one);
        path.AddWaypoint(Vector2.left);

        Assert.AreEqual(Vector2.left, path.GetNewestWaypoint().Position);
    }

    [Test, Timeout(5000)]
    public void SnakePathGetNextWaypoint()
    {
        SnakePath path = new SnakePath(3);

        path.AddWaypoint(Vector2.zero);
        Waypoint first = path.GetNewestWaypoint();
        Assert.AreEqual(null, path.GetNextWaypoint(first));

        path.AddWaypoint(Vector2.one);
        Waypoint second = path.GetNewestWaypoint();
        Assert.AreEqual(second, path.GetNextWaypoint(first));
        Assert.AreEqual(null, path.GetNextWaypoint(second));

        path.AddWaypoint(Vector2.left);
        Waypoint third = path.GetNewestWaypoint();
        Assert.AreEqual(second, path.GetNextWaypoint(first));
        Assert.AreEqual(third, path.GetNextWaypoint(second));
        Assert.AreEqual(null, path.GetNextWaypoint(third));

        path.AddWaypoint(Vector2.down);
        Waypoint fourth = path.GetNewestWaypoint();
        Assert.AreEqual(null, path.GetNextWaypoint(first));
        Assert.AreEqual(third, path.GetNextWaypoint(second));
        Assert.AreEqual(fourth, path.GetNextWaypoint(third));
        Assert.AreEqual(null, path.GetNextWaypoint(fourth));
    }

}
