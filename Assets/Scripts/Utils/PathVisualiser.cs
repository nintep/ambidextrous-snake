using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for debugging snake paths
public class PathVisualiser : MonoBehaviour
{
    public GameObject WaypointGraphic;
    public float RefreshRate;
    public bool Visualise;

    private float refreshDelayRemaining;
    private SnakePath path;
    private List<GameObject> graphics;
    private Queue<GameObject> graphicsPool;

    private void Start()
    {
        Snake snake = GetComponent<Snake>();
        VisualisePath(snake.Path);
    }

    private void VisualisePath(SnakePath path)
    {
        if (!Visualise)
        {
            return;
        }

        graphics = new List<GameObject>();
        graphicsPool = new Queue<GameObject>();

        this.path = path;
        refreshDelayRemaining = 0;

        foreach (GameObject graphic in graphics)
        {
            graphic.SetActive(false);
            graphicsPool.Enqueue(graphic);
        }
        graphics.Clear();

        for (int i = 0; i < path.Length; i++)
        {
            GameObject waypoint = GetUnusedWaypointGraphic();
            graphics.Add(waypoint);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Visualise)
        {
            return;
        }

        refreshDelayRemaining -= Time.deltaTime;
        refreshDelayRemaining = Mathf.Max(refreshDelayRemaining, 0);

        if (refreshDelayRemaining <= 0)
        {
            UpdateGraphicPositions();
            refreshDelayRemaining = RefreshRate;
        }
    }

    // Gets an unused graphic from the pool, or creates one if pool is empty
    private GameObject GetUnusedWaypointGraphic()
    {
        if (graphicsPool.Count == 0)
        {
            GameObject waypoint = GameObject.Instantiate(WaypointGraphic);
            return waypoint;
        }
        else
        {
            GameObject waypoint = graphicsPool.Dequeue();
            waypoint.SetActive(true);
            return waypoint;
        }
    }

    private void UpdateGraphicPositions()
    {
        // Get new graphics if necessary
        while (graphics.Count < path.Length)
        {
            GameObject waypoint = GetUnusedWaypointGraphic();
            graphics.Add(waypoint);
        }

        // Remove unnecessary graphics
        while (graphics.Count > path.Length)
        {
            GameObject graphic = graphics[graphics.Count - 1];
            graphics.Remove(graphic);
            graphic.SetActive(false);
            graphicsPool.Enqueue(graphic);
        }

        List<Waypoint> waypoints = path.GetWaypoints();

        for (int i = 0; i < path.Length; i++)
        {
            graphics[i].transform.position = waypoints[i].Position;
        }
    }
}
