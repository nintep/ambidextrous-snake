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
            yield return new WaitForEndOfFrame();
        }

        // A few extra frames for setup
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }

    [UnityTearDown]
    public IEnumerator Teardown() {
        yield return new WaitForEndOfFrame();
    }

    [UnityTest, Timeout(10000)]
    public IEnumerator TestSceneContainsPlayer()
    {
        Debug.Log("--- Running TestSceneContainsPlayer");

        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        yield return new WaitForEndOfFrame();
    }

    [UnityTest, Timeout(10000)]
    public IEnumerator PlayerStaysStillBeforeStarted()
    {
        Debug.Log("--- Running PlayerStaysStillBeforeStarted");

        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(playerStartPosition, player.transform.position);
    }

    [UnityTest, Timeout(10000)]
    public IEnumerator PlayerMovesUpAfterStarted()
    {
        Debug.Log("--- Running PlayerMovesUpAfterStarted");

        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;
        player.SetStartDirection(Vector2.up);
        player.StartMovement();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(player.transform.position.x, playerStartPosition.x);
        Assert.Greater(player.transform.position.y, playerStartPosition.y);
    }

    [UnityTest, Timeout(10000)]
    public IEnumerator PlayerMoveDirectionChangesOnInput()
    {
        Debug.Log("--- Running PlayerMoveDirectionChangesOnInput");

        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;
        player.SetStartDirection(Vector2.left);
        player.StartMovement();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Check that player moved left
        Assert.AreEqual(player.transform.position.y, playerStartPosition.y);
        Assert.Less(player.transform.position.x, playerStartPosition.x);

        playerStartPosition = player.transform.position;
        player.AddMoveInput(Vector2.down);
        yield return new WaitForEndOfFrame();

        // Check that player moved down
        Assert.AreEqual(player.transform.position.x, playerStartPosition.x);
        Assert.Less(player.transform.position.y, playerStartPosition.y);
    }    
}
