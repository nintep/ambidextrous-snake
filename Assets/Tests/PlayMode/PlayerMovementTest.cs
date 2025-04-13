using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerMovementTest
{
    // Index of scene used for testing
    private int sceneIdx = 0;

    [UnitySetUp]
    public IEnumerator Setup() {
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Single);
        while (SceneManager.GetActiveScene().buildIndex != sceneIdx)
        {
            yield return null;
        }

        // A few extra frames for setup
        yield return null;
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown() {
        yield return null;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator PlayerStaysStillBeforeStarted()
    {
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;

        yield return null;

        Assert.AreEqual(playerStartPosition, player.transform.position);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator PlayerMovesUpAfterStarted()
    {
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;
        player.SetStartDirection(Vector2.up);
        player.StartMovement();

        yield return null;

        Assert.AreEqual(player.transform.position.x, playerStartPosition.x);
        Assert.Greater(player.transform.position.y, playerStartPosition.y);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator PlayerMoveDirectionChangesOnInput()
    {
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;
        player.SetStartDirection(Vector2.left);
        player.StartMovement();

        yield return null;

        // Check that player moved left
        Assert.AreEqual(player.transform.position.y, playerStartPosition.y);
        Assert.Less(player.transform.position.x, playerStartPosition.x);

        playerStartPosition = player.transform.position;
        player.AddMoveInput(Vector2.down);

        yield return null;

        // Check that player moved down
        Assert.AreEqual(player.transform.position.x, playerStartPosition.x);
        Assert.Less(player.transform.position.y, playerStartPosition.y);
    }    
}
