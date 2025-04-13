using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;


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
    public void SceneContainsPlayer()
    {
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);
    }

    [Test, Timeout(5000)]
    public void SceneContainsSnakeSegment()
    {
        SnakeSegment segment = GameObject.FindFirstObjectByType<SnakeSegment>();
        Assert.IsNotNull(segment);
    }

    [Test, Timeout(5000)]
    public void SceneContainsCamera()
    {
        Camera camera = GameObject.FindFirstObjectByType<Camera>();
        Assert.IsNotNull(camera);
    }
}
