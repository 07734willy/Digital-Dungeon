using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour {

    public enum Status {
        waiting,
        running,
        passed,
        failed
    }

    public List<IntegrationTest> tests;
    public string nextSceneName;
    private string currentSceneName;
    private int testIndex;
    private bool testStarted = false;

    public void Awake() {
        this.currentSceneName = SceneManager.GetActiveScene().name;
        this.testIndex = PlayerPrefs.GetInt("testIndex", 0);
    }

	
	void Update () {
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
            StartCoroutine(tests[testIndex].Run());
            StartCoroutine(TestTimeoutFail(tests[testIndex].timeout));
            return;
        }

        Status status = tests[testIndex].status; 
        if (status == Status.failed) {
            Debug.LogError(string.Format("[FAILED] Test {0} in scene {1} failed\n  Description: {2}", testIndex, currentSceneName, tests[testIndex].description));
            RunNextTest();
        } else if (status == Status.passed) {
            Debug.Log(string.Format("[PASSED] Test {0} in scene {1} passed", testIndex, currentSceneName));
            RunNextTest();
        }
    }

    IEnumerator TestTimeoutFail(int timeout) {
        yield return new WaitForSeconds(timeout);
        tests[testIndex].status = Status.failed;
        yield return null;
    }

    private void RunNextTest() {
        PlayerPrefs.SetInt("testIndex", testIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
