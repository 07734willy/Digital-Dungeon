using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour {

    public enum Status {
        waiting,
        running,
        passed,
        failed,
        timedout
    }

    public string nextSceneName;
    private string currentSceneName;
    private int testIndex;
    private bool testStarted = false;
    private List<IntegrationTest> tests;

    public void Awake() {
        this.currentSceneName = SceneManager.GetActiveScene().name;
        this.testIndex = PlayerPrefs.GetInt("testIndex", 0);
    }

    public void Start() {
        this.tests = new List<IntegrationTest>(this.gameObject.GetComponentsInChildren<IntegrationTest>());
    }

    public IEnumerator DelayedStartup() {
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(tests[testIndex].Run());
        StartCoroutine(TestTimeoutFail(tests[testIndex].timeout));
        yield return null;
    }

    void Update() {
        if (this.testIndex >= this.tests.Count) {
            PlayerPrefs.DeleteKey("testIndex");
            if (nextSceneName != "") {
                SceneManager.LoadScene(nextSceneName);
            } else {
                Application.Quit();
#if UNITY_EDITOR
                //Stop playing the scene
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            return;
        }

        if (!testStarted) {
            testStarted = true;
            StartCoroutine(DelayedStartup());
            return;
        }

        Status status = tests[testIndex].status;
        if (status == Status.failed) {
            Debug.LogError(string.Format("[FAILED] Test {0} in scene {1} failed\n  Description: {2}", testIndex, currentSceneName, tests[testIndex].description));
            RunNextTest();
        } else if (status == Status.passed) {
            Debug.Log(string.Format("[PASSED] Test {0} in scene {1} passed", testIndex, currentSceneName));
            RunNextTest();
        } else if (status == Status.timedout) {
            Debug.LogError(string.Format("[FAILED] Test {0} in scene {1} timed out after {2} seconds\n  Description: {3}", testIndex, currentSceneName, tests[testIndex].timeout, tests[testIndex].description));
            RunNextTest();
        }
    }

    IEnumerator TestTimeoutFail(int timeout) {
        yield return new WaitForSeconds(timeout);
        tests[testIndex].status = Status.timedout;
        yield return null;
    }

    private void RunNextTest() {
        PlayerPrefs.SetInt("testIndex", testIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
