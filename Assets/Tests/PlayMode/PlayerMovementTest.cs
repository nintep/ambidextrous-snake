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
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSceneContainsPlayer()
    {        
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerStaysStillBeforeStarted()
    {
        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        Vector3 playerStartPosition = player.transform.position;
        yield return null;

        Assert.AreEqual(playerStartPosition, player.transform.position);
    }

    [UnityTest]
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

    [UnityTest]
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


    /* private PlayerMovement player;

    private IEnumerator SetupMainScene()
    {
        SceneManager.LoadScene("Assets/Scenes/Main.unity", LoadSceneMode.Single);
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }

        player = GameObject.FindFirstObjectByType<PlayerMovement>();
    }

    [UnityTest]
    public IEnumerator LoadMainScene()
    {
        SceneManager.LoadScene("Assets/Scenes/Main.unity", LoadSceneMode.Single);
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }

    [UnityTest]
    public IEnumerator PlayerStaysStillBeforeStarted()
    {
        yield return SetupMainScene();
        Assert.IsNotNull(player);

        Vector2 playerStartPosition = player.transform.position;
        yield return null;

        Assert.AreEqual(playerStartPosition, player.transform.position);
    }

    [UnityTest]
    public IEnumerator PlayerMovesUpAfterStarted()
    {
        Assert.IsNotNull(player);

        Vector2 playerStartPosition = player.transform.position;
        player.SetStartDirection(Vector2.up);
        player.StartMovement();

        yield return null;

        Assert.AreEqual(player.transform.position.x, playerStartPosition.x);
        Assert.Greater(player.transform.position.y, playerStartPosition.y);
    } */


    
}
