using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class BugTests
    {
        [UnityTest]
        public IEnumerator OverlappedEnemies()
        {
            // Setup
            SceneManager.LoadScene("Bug_OverlappedEnemies");



            yield return null;

            // Assert.IsTrue();
        }
    }
}
