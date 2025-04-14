using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using System.Linq;


public class MainSceneContentTests
{
    // Index of scene used for testing
    private string scenePath = "Assets/Scenes/Main.unity";

    [UnitySetUp]
    public IEnumerator Setup() {
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        
        while (EditorSceneManager.GetActiveScene().name != "Main")
        {
            yield return null;
        }

        // A few extra frames for setup
        yield return null;
        yield return null;
    }

    [Test, Timeout(5000)]
    public void SceneContainsTwoPlayerMovements()
    {
        PlayerMovement[] playerMovements = GameObject.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.InstanceID);
        Assert.AreEqual(2, playerMovements.Count());
    }

    [Test, Timeout(5000)]
    public void SceneContainsTwoSnakes()
    {
        Snake[] snakes = GameObject.FindObjectsByType<Snake>(FindObjectsSortMode.InstanceID);
        Assert.AreEqual(2, snakes.Count());

        PlayerType snake_1_type = snakes[0].GetComponent<PlayerMovement>().PlayerType;
        PlayerType snake_2_type = snakes[1].GetComponent<PlayerMovement>().PlayerType;

        Assert.AreNotEqual(snake_1_type, snake_2_type);
    }

    [Test, Timeout(5000)]
    public void SceneContainsTwoSnakeSegments()
    {
        SnakeSegment[] segments = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID);
        Assert.AreEqual(2, segments.Count());

        //Check that segments belong to different game objects
        Assert.AreNotEqual(segments[0].gameObject, segments[1].gameObject);
    }

    [Test, Timeout(5000)]
    public void SceneContainsCamera()
    {
        Camera camera = GameObject.FindFirstObjectByType<Camera>();
        Assert.IsNotNull(camera);
    }
}
