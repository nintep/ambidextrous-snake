using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ci_test_script
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

        yield return null;
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown() {
        yield return new WaitForSeconds(0.01f);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator ci_TestSceneContainsPlayer()
    {
        Debug.Log("--- Running ci_TestSceneContainsPlayer");

        PlayerMovement player = GameObject.FindFirstObjectByType<PlayerMovement>();
        Assert.IsNotNull(player);

        yield return null;
    }
}
